import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommandeService } from '../commande.service';
import { PharmacieService } from '../../transaction/pharmacies/pharmacie.service';
import { AllProductsService } from '../../produits/all-products/all-products.service';
import { GetDepotsService } from '../../depots/get-depots/get-depots.service';
import { CreateCommandeDto, StatusCommande, UpdateCommandeDto, LigneCommandeDto, AllocationDepotDto } from '../models/commande.models';
import { ProductFilterRequest } from 'src/app/produits/dto/ProductFilterRequest';
import { CloudinaryService } from '../../shared/services/cloudinary.service';

@Component({
  selector: 'app-add-update-commande',
  templateUrl: './add-update-commande.component.html',
  styleUrls: ['./add-update-commande.component.scss']
})
export class AddUpdateCommandeComponent implements OnInit {
  commandeForm!: FormGroup;
  isEditMode = false;
  commandeId: string | null = null;
  loading = false;
  submitting = false;
  error: string | null = null;

  // Dropdown data
  pharmacies: any[] = [];
  filteredPharmacies: any[] = [];
  products: any[] = [];
  depots: any[] = [];

  // Search
  pharmacySearchTerm: string = '';

  // File upload
  selectedFile: File | null = null;

  // Enums
  StatusCommande = StatusCommande;
  statusOptions = Object.values(StatusCommande);

  availableDepots: Map<number, any[]> = new Map(); // Key: line index, Value: List of depots with stock
  maxDate: string = new Date().toISOString().substring(0, 10);

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private commandeService: CommandeService,
    private pharmacieService: PharmacieService,
    private productService: AllProductsService,
    private depotService: GetDepotsService,
    private cloudinaryService: CloudinaryService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadDropdownData();

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.commandeId = id;
        this.loadCommande(id);
      }
    });

    // Generate random code for new orders
    if (!this.isEditMode) {
      this.commandeForm.patchValue({
        codeComm: Math.floor(100000 + Math.random() * 900000)
      });
    }
  }

  initForm(): void {
    this.commandeForm = this.fb.group({
      pharmacieId: ['', Validators.required],
      orderDate: [new Date().toISOString().substring(0, 10), Validators.required],
      status: [StatusCommande.EnAttente, Validators.required],
      codeComm: ['', Validators.required],
      lines: this.fb.array([])
    });
  }

  get lines(): FormArray {
    return this.commandeForm.get('lines') as FormArray;
  }

  createLine(): FormGroup {
    return this.fb.group({
      produitId: ['', Validators.required],
      uniteProduitPrix: [0, [Validators.required, Validators.min(0)]],
      allocationsDepots: this.fb.array([])
    });
  }

  addLine(): void {
    const line = this.createLine();
    // Add one default allocation
    this.addAllocation(line.get('allocationsDepots') as FormArray, this.lines.length);
    this.lines.push(line);
  }

  removeLine(index: number): void {
    this.lines.removeAt(index);
    this.availableDepots.delete(index);
    // Re-index map keys if necessary? 
    // Ideally we should rely on a unique ID for the line, but index is tricky if we remove from middle.
    // Simplifying assumption: Resetting/Reloading might be safer, but for now let's just delete.
    // Actually, shifting indices in a Map is painful. 
    // Better approach: When rendering, we use 'i' from *ngFor. If we remove index 1, index 2 becomes 1.
    // So we must shift the Map entries.
    this.shiftMapIndices(index);
  }

  shiftMapIndices(removedIndex: number): void {
      const newMap = new Map<number, any[]>();
      this.availableDepots.forEach((value, key) => {
          if (key < removedIndex) {
              newMap.set(key, value);
          } else if (key > removedIndex) {
              newMap.set(key - 1, value);
          }
      });
      this.availableDepots = newMap;
  }

  getAllocations(lineIndex: number): FormArray {
    return this.lines.at(lineIndex).get('allocationsDepots') as FormArray;
  }

  createAllocation(maxQuantity?: number): FormGroup {
    const validators = [Validators.required, Validators.min(1)];
    // We can't easily add max validator here because it depends on the selected depot...
    // We will handle the max validation dynamically when depot changes.
    return this.fb.group({
      depotId: ['', Validators.required],
      quantityAllocated: [1, validators]
    });
  }

  addAllocation(allocations: FormArray, lineIndex: number): void {
    allocations.push(this.createAllocation());
  }

  removeAllocation(lineIndex: number, allocIndex: number): void {
    this.getAllocations(lineIndex).removeAt(allocIndex);
  }

  loadDropdownData(): void {
    // Load Pharmacies
    this.pharmacieService.getPharmacies({ pageIndex: 0, pageSize: 100 }).subscribe({
      next: (res) => {
        this.pharmacies = res.items || res.data || [];
        this.pharmacies = res.items || res.data || [];
        this.filterPharmacies(); // Apply filter in case search term was set by loadCommande
      },
      error: (err) => console.error('Error loading pharmacies', err)
    });
const payload: ProductFilterRequest={
  filter:{
   categorieId: null, 
            name: null, 
            categorieName: null, 
            minPrixVente: null, 
            maxPrixVente: null    
  },
  pageNumber:1,
  pageSize:100
}
    // Load Products
    this.productService.getProducts(payload).subscribe({
      next: (res) => this.products = res.paginationResult.data || res.data || [],
      error: (err) => console.error('Error loading products', err)
    });

    // Depots are loaded per product now
  }

  onProductSelect(lineIndex: number): void {
    const line = this.lines.at(lineIndex);
    const productId = line.get('produitId')?.value;
    const product = this.products.find(p => p.id === productId);
    
    if (product) {
      line.patchValue({
        uniteProduitPrix: product.prixVente
      });

      // Fetch depots for this product
      this.depotService.getDepotsStockForProduct(productId).subscribe({
        next: (res) => {
            const stockDepots = res.items || res.data || res || []; // Adapt based on actual API response structure
            this.availableDepots.set(lineIndex, stockDepots);
            
            // Clear existing allocations or reset their depot selection?
            // Maybe just warn user? For now, we assume user is picking things sequentially.
        },
        error: (err) => console.error('Error loading depots for product', err)
      });
    }
  }

  onDepotSelect(lineIndex: number, allocIndex: number): void {
      const allocations = this.getAllocations(lineIndex);
      const allocGroup = allocations.at(allocIndex);
      const depotId = allocGroup.get('depotId')?.value;
      
      const depotsForLine = this.availableDepots.get(lineIndex) || [];
      const selectedDepot = depotsForLine.find(d => d.depotId === depotId);

      if (selectedDepot) {
          const maxQty = selectedDepot.quantity;
          const qtyControl = allocGroup.get('quantityAllocated');
          
          qtyControl?.setValidators([Validators.required, Validators.min(1), Validators.max(maxQty)]);
          qtyControl?.updateValueAndValidity();
      }
  }

  calculateLineTotal(lineIndex: number): number {
    const line = this.lines.at(lineIndex);
    const price = line.get('uniteProduitPrix')?.value || 0;
    const allocations = (line.get('allocationsDepots') as FormArray).controls;
    
    let totalQty = 0;
    allocations.forEach(alloc => {
      totalQty += alloc.get('quantityAllocated')?.value || 0;
    });
    
    return price * totalQty;
  }

  calculateTotalAmount(): number {
    let total = 0;
    this.lines.controls.forEach((_, index) => {
      total += this.calculateLineTotal(index);
    });
    return total;
  }

  loadCommande(id: string): void {
    this.loading = true;
    this.commandeService.getCommandeById(id).subscribe({
      next: (commande) => {
        if (!commande) {
          this.error = 'Commande introuvable';
          this.loading = false;
          return;
        }

        // Patch basic fields
        this.commandeForm.patchValue({
          pharmacieId: commande.pharamcieInfo.id,
          orderDate: commande.orderDate.substring(0, 10),
          status: commande.status,
          codeComm: commande.codeComm
        });

        // Set pharmacy name for display
        if (commande.pharamcieInfo?.name) {
            this.pharmacySearchTerm = commande.pharamcieInfo.name;
            this.filterPharmacies();
        }

        // Clear existing lines
        while (this.lines.length !== 0) {
          this.lines.removeAt(0);
        }

        // Add lines from commande
        commande.lines.forEach((lineItem, index) => {
          const lineGroup = this.createLine();
          
          // Load depots for this product
          this.depotService.getDepotsStockForProduct(lineItem.produitId).subscribe({
               next: (res) => {
                   const stockDepots = res.items || res.data || res || [];
                   this.availableDepots.set(index, stockDepots);
                   
                    // Re-run validation for allocations now that we have stock info
                    const allocationsArray = lineGroup.get('allocationsDepots') as FormArray;
                    allocationsArray.controls.forEach(allocGroup => {
                         const depotId = allocGroup.get('depotId')?.value;
                         const selectedDepot = stockDepots.find((d: any) => d.depotId === depotId);
                         if (selectedDepot) {
                            const qtyControl = allocGroup.get('quantityAllocated');
                            qtyControl?.setValidators([Validators.required, Validators.min(1), Validators.max(selectedDepot.quantity)]);
                            qtyControl?.updateValueAndValidity();
                         }
                    });
               }
          });

          // Recreate allocations
          const allocationsArray = lineGroup.get('allocationsDepots') as FormArray;
          lineItem.allocationsDepots.forEach(alloc => {
            const allocGroup = this.createAllocation();
            allocGroup.patchValue({
              depotId: alloc.depotInfo.id,
              quantityAllocated: alloc.quantityAllocated
            });
            allocationsArray.push(allocGroup);
          });

          lineGroup.patchValue({
            produitId: lineItem.produitId,
            uniteProduitPrix: lineItem.uniteProduitPrix
          });

          this.lines.push(lineGroup);
        });

        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erreur lors du chargement de la commande';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.commandeForm.invalid) {
        this.commandeForm.markAllAsTouched();
        return;
    }

    this.submitting = true;

    if (this.selectedFile) {
        this.cloudinaryService.uploadFile(this.selectedFile).subscribe({
            next: (url) => {
                this.processSubmission(url);
            },
            error: (err) => {
                this.error = 'Erreur lors du téléchargement du fichier sur Cloudinary';
                console.error(err);
                this.submitting = false;
            }
        });
    } else {
        this.processSubmission('');
    }
  }

  private processSubmission(documentUrl: string): void {
    const formValue = this.commandeForm.value;

    const linesDto: LigneCommandeDto[] = formValue.lines.map((line: any) => {
        const product = this.products.find(p => p.id === line.produitId);
        return {
            produitId: line.produitId,
            nomProuit: product ? product.name : 'Unknown Product',
            uniteProduitPrix: line.uniteProduitPrix,
            allocationsDepots: line.allocationsDepots.map((alloc: any) => {
                const depot = this.depots.find(d => d.id === alloc.depotId);
                return {
                    depotId: alloc.depotId,
                    depotName: depot ? depot.name : 'Unknown Depot', 
                    quantityAllocated: alloc.quantityAllocated
                };
            })
        };
    });

    if (this.isEditMode && this.commandeId) {
        const updateDto: UpdateCommandeDto = {
            status: formValue.status,
            lines: linesDto,
            originalDocumentPath: documentUrl || undefined // Only update if we have a new URL? Or logic depending on requirements.
        };

        this.commandeService.updateCommande(this.commandeId, updateDto).subscribe({
            next: () => {
                this.router.navigate(['/commandes']);
            },
            error: () => {
                this.error = 'Erreur lors de la mise à jour';
                this.submitting = false;
            }
        });
    } else {
        // Need pharmacy info for Create DTO
        const pharmacy = this.pharmacies.find(p => p.id === formValue.pharmacieId);
        const userInfo = { id: 'a127521e-d4e5-4a65-9c86-9ac324220794', fullName: 'Current User' }; 

        const createDto: CreateCommandeDto = {
            pharamcieInfo: {
                id: formValue.pharmacieId,
                name: pharmacy ? pharmacy.name : 'Unknown',
                address: pharmacy && pharmacy.location ? pharmacy.location.gouvernerat : undefined
            },
            userInfo: userInfo,
            codeComm: formValue.codeComm,
            lines: linesDto,
            originalDocumentPath: documentUrl
        };

        this.commandeService.createCommande(createDto).subscribe({
            next: () => {
                this.router.navigate(['/commandes']);
            },
            error: (err) => {
                this.error = 'Erreur lors de la création';
                console.error(err);
                this.submitting = false;
            }
        });
    }
  }

  cancel(): void {
    this.router.navigate(['/commandes']);
  }

  // Pharmacy search filter
  filterPharmacies(): void {
    if (!this.pharmacySearchTerm) {
      this.filteredPharmacies = this.pharmacies;
      return;
    }

    const term = this.pharmacySearchTerm.toLowerCase();
    this.filteredPharmacies = this.pharmacies.filter(p => 
      p.name?.toLowerCase().includes(term) ||
      p.location?.gouvernerat?.toLowerCase().includes(term) ||
      p.location?.delegation?.toLowerCase().includes(term)
    );
  }

  // File handling
  onFileSelect(event: any): void {
    const file = event.target.files[0];
    if (file) {
      // Check file size (5MB max)
      const maxSize = 5 * 1024 * 1024; // 5MB
      if (file.size > maxSize) {
        this.error = 'Le fichier est trop volumineux. Taille maximale: 5MB';
        return;
      }

      // Check file type
      const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'application/pdf'];
      if (!allowedTypes.includes(file.type)) {
        this.error = 'Type de fichier non autorisé. Utilisez JPG, PNG ou PDF';
        return;
      }

      this.selectedFile = file;
      this.error = null;
      console.log('File selected:', file.name, file.size, file.type);
      // TODO: Upload file to server or store it for later upload
    }
  }

  clearFile(): void {
    this.selectedFile = null;
    // Reset file input
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  selectPharmacy(id: string): void {
    this.commandeForm.patchValue({ pharmacieId: id });
    this.commandeForm.get('pharmacieId')?.markAsTouched();
  }

  // --- Custom Dropdown Logic ---
  activeDropdown: string | null = null;
  productSearch: string = '';
  depotSearch: string = '';
  filteredProducts: any[] = [];
  filteredDepots: any[] = []; // Current filtered list for the active depot dropdown

  toggleDropdown(id: string): void {
    if (this.activeDropdown === id) {
      this.closeDropdowns();
    } else {
      this.activeDropdown = id;
      this.productSearch = '';
      this.depotSearch = '';
      
      // Initialize filters based on what we are opening
      if (id.startsWith('product')) {
        this.filteredProducts = this.products;
      } else if (id.startsWith('depot')) {
          // extract indices
          const parts = id.split('-');
          const lineIndex = parseInt(parts[1], 10);
          const allocIndex = parseInt(parts[2], 10);
          
          this.updateFilteredDepots(lineIndex, allocIndex);
      }
    }
  }

  updateFilteredDepots(lineIndex: number, allocIndex: number): void {
      const sourceList = this.availableDepots.get(lineIndex) || [];
      
      // Get selected depot IDs in other allocations of this line
      const allocations = this.getAllocations(lineIndex);
      const selectedIds = allocations.controls
        .map((ctrl, idx) => idx !== allocIndex ? ctrl.get('depotId')?.value : null)
        .filter(id => id); // Remove nulls/undefined
      
      // Filter source list: Exclude if in selectedIds
      const validDepots = sourceList.filter(d => !selectedIds.includes(d.depotId));

      // Apply search term if any
      if (this.depotSearch) {
          const term = this.depotSearch.toLowerCase();
          this.filteredDepots = validDepots.filter(d => d.depotName.toLowerCase().includes(term));
      } else {
          this.filteredDepots = validDepots;
      }
  }

  closeDropdowns(): void {
    this.activeDropdown = null;
  }

  filterProductsLocal(): void {
    if (!this.productSearch) {
      this.filteredProducts = this.products;
      return;
    }
    const term = this.productSearch.toLowerCase();
    this.filteredProducts = this.products.filter(p => 
      p.name.toLowerCase().includes(term)
    );
  }

  filterDepotsLocal(): void {
      if (!this.activeDropdown) return;
      const parts = this.activeDropdown.split('-');
      const lineIndex = parseInt(parts[1], 10);
      const allocIndex = parseInt(parts[2], 10);
      
      this.updateFilteredDepots(lineIndex, allocIndex);
  }

  selectProduct(lineIndex: number, productId: string): void {
      const line = this.lines.at(lineIndex);
      line.patchValue({ produitId: productId });
      this.onProductSelect(lineIndex); // Trigger logic
      this.closeDropdowns();
  }

  selectDepot(lineIndex: number, allocIndex: number, depotId: string): void {
      const allocations = this.getAllocations(lineIndex);
      const allocGroup = allocations.at(allocIndex);
      allocGroup.patchValue({ depotId: depotId });
      this.onDepotSelect(lineIndex, allocIndex); // Trigger logic
      this.closeDropdowns();
  }
  
  // Helper to get product name for display
  getProductName(id: string): string {
      const p = this.products.find(x => x.id === id);
      return p ? p.name : 'Sélectionner un produit';
  }

  getDepotName(lineIndex: number, id: string): string {
      const list = this.availableDepots.get(lineIndex) || [];
      const d = list.find(x => x.depotId === id);
      return d ? d.depotName : 'Choisir dépôt';
  }
}
