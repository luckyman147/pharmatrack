import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DepotService } from '../services/depot.service';
import { Product } from 'src/app/produits/models/Product';
import { CreateDepotDto } from '../dto/create-depot.dto';
import { Depot } from '../models/depot';
import { AllProductsService } from 'src/app/produits/all-products/all-products.service';

@Component({
  selector: 'app-add-update-depot',
  templateUrl: './add-update-depot.component.html',
  styleUrls: ['./add-update-depot.component.scss'],
})
export class AddUpdateDepotComponent implements OnInit {
  @Input() depot: Depot | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  depotForm!: FormGroup;
  availableProducts: Product[] = [];
  isLoading = false;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private depotService: DepotService,
    private produitsService: AllProductsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadProducts();
  }

  private initForm(): void {
    this.depotForm = this.fb.group({
      name: ['', Validators.required],
      telephone: ['', Validators.required],
      rapportDocumentUrl: [''],
      location: this.fb.group({
        rue: [''],
        gouvernerat: [''],
        delegation: [''],
        zone: ['']
      }),
      produitStock: this.fb.array([])
    });
  }

  get productsArray(): FormArray {
    return this.depotForm.get('produitStock') as FormArray;
  }

  private loadProducts(): void {
    this.isLoading = true;
    const payload: any = {
      filter: {
        categorieId: null,
        name: null,
        categorieName: null,
        minPrixVente: null,
        maxPrixVente: null
      },
      pageNumber: 1,
      pageSize: 100
    };

    this.produitsService.getProducts(payload).subscribe({
      next: (res) => {
        const products = res.paginationResult?.data || res.data || [];
        this.availableProducts = products;
        this.populateProductFormArray();
        this.checkRouteForId(); // Check for ID after products are loaded
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading products', err);
        this.isLoading = false;
      }
    });
  }

  private checkRouteForId(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.isLoading = true;
      this.depotService.getDepot(id).subscribe({
        next: (response) => {
          // Handle loose response type from previous changes (response.depot or response)
          this.depot = response.depot || response; 
          if (this.depot) {
            this.patchForm(this.depot);
          }
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error loading depot for edit', err);
          this.isLoading = false;
        }
      });
    } else if (this.depot) {
      // Modal mode with pre-supplied depot
      this.isEditMode = true;
      this.patchForm(this.depot);
    }
  }

  private populateProductFormArray(): void {
    this.productsArray.clear();
    this.availableProducts.forEach(product => {
      this.productsArray.push(this.fb.group({
        produitId: [product.id],
        name: [product.name],
        quantity: [0, [Validators.min(0)]]
      }));
    });
  }

  private patchForm(depot: Depot): void {
    this.depotForm.patchValue({
      name: depot.name,
      telephone: depot.telephone,
      rapportDocumentUrl: depot.rapportDocumentUrl
    });

    if (depot.location) {
      this.depotForm.get('location')?.patchValue({
        rue: depot.location.rue,
        gouvernerat: depot.location.gouvernerat,
        delegation: depot.location.delegation,
        zone: depot.location.zone
      });
    }

    if (depot.produitStock && depot.produitStock.length > 0) {
      depot.produitStock.forEach((item: any) => {
        const productControl = this.productsArray.controls.find(
          ctrl => ctrl.get('produitId')?.value === (item.produitId || item.id)
        );
        if (productControl) {
          productControl.patchValue({ quantity: item.quantity });
        }
      });
    }
  }

  saveChanges(): void {
    if (this.depotForm.invalid) {
      this.depotForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const formValue = this.depotForm.value;
    
    // Filter products with quantity > 0
    const selectedProducts = formValue.produitStock
      .filter((p: any) => p.quantity > 0)
      .map((p: any) => ({
        produitId: p.produitId,
        quantity: p.quantity
      }));

    const dto: CreateDepotDto = {
      name: formValue.name,
      telephone: formValue.telephone,
      rapportDocumentUrl: formValue.rapportDocumentUrl,
      location: formValue.location,
      produitStock: selectedProducts
    };

    if (this.depot && this.depot.id) {
      this.depotService.updateDepot(this.depot.id, dto).subscribe({
        next: () => {
          this.isLoading = false;
          this.saved.emit();
          this.closeModal();
        },
        error: (err) => {
          console.error('Error updating depot', err);
          this.isLoading = false;
        }
      });
    } else {
      this.depotService.createDepot(dto).subscribe({
        next: () => {
          this.isLoading = false;
          this.saved.emit();
          this.closeModal();
        },
        error: (err) => {
          console.error('Error creating depot', err);
          this.isLoading = false;
        }
      });
    }
  }

  closeModal(): void {
    if (this.route.snapshot.paramMap.get('id')) {
        // Navigating back if routed
        this.router.navigate(['..'], { relativeTo: this.route });
    } else {
        // Modal close
        this.close.emit();
    }
  }
}
