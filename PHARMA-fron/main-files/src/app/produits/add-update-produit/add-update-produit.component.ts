import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AddUpdateProduitService } from './add-update-produit.service';
import Swal from 'sweetalert2';
import { AllCategoriesService } from '../categorie/all-categories/all-categories.service';
import { CreateProductRequestDto } from '../dto/CreateProductRequestDto';
import { id } from 'date-fns/locale';
import { UpdateProductRequestDto } from '../dto/UpdateProductRequestDTo';

@Component({
  selector: 'app-add-update-produit',
  templateUrl: './add-update-produit.component.html',
  styleUrls: ['./add-update-produit.component.scss'],
})
export class AddUpdateProduitComponent implements OnInit {
  productForm!: FormGroup;
  categories: any[] = [];
  isLoading = true;
  isUpdateMode = false;
  productData: any;
  constructor(
    private fb: FormBuilder,
    private addUpdateService: AddUpdateProduitService,
    private router: Router,
    private categorieService: AllCategoriesService
  ) {}

  ngOnInit(): void {
    const navState = history.state;
    this.productData = navState.productData;
    this.isUpdateMode = navState.isUpdated || false;
    if (this.isUpdateMode && this.productData) {
          this.productForm = this.fb.group({
            name: [this.productData?.name || '', Validators.required],
            description: [
              this.productData?.description || '',
              Validators.required,
            ],
            prixVente: [
              this.productData?.prixVente || null,
              [Validators.required, Validators.min(0)],
            ],
            categorieId: [
              this.productData?.categorie.id || '',
              Validators.required,
            ],
          });
    }

    else {
    // Initialize empty form immediately to avoid template errors
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      prixVente: [null, [Validators.required, Validators.min(0)]],
      categorieId: ['', Validators.required],
    });
  }
    if (navState.categories && navState.categories.length > 0) {
      this.categories = navState.categories;
      this.isLoading = false;
    } else {
      // Fetch categories from service if not passed
      this.categorieService.getCategories().subscribe({
        next: (cats) => {
          this.categories = cats;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to fetch categories', err);
          this.isLoading = false;
        },
      });
    }
  }
  onUpdate(): void {
    if (this.productForm.invalid) {
      Swal.fire({
        icon: 'warning',
        title: 'Champs requis',
        text: 'Veuillez remplir tous les champs correctement.',
      });
      return;
    }

    const dto: UpdateProductRequestDto = {
      ...this.productForm.value,

    };
    console.log('Update DTO:', dto);

    const productId = this.productData.id;

    Swal.fire({
      title: 'Mise à jour du produit...',
      allowOutsideClick: false,
      allowEscapeKey: false,
      showConfirmButton: false,
      didOpen: () => Swal.showLoading(),
    });

    this.addUpdateService.updateProduct(productId, dto).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Succès',
          text: 'Produit mis à jour avec succès !',
          confirmButtonText: 'OK',
        }).then(() => this.router.navigate(['/produits']));
      },
      error: (err) => {
        console.error('Erreur lors de la mise à jour du produit:', err);
        Swal.fire({
          icon: 'error',
          title: 'Échec',
          text: 'Impossible de mettre à jour le produit. Veuillez réessayer.',
        });
      },
    });
  }

  onSubmit(): void {
if (this.isUpdateMode) {
      this.onUpdate();
    
    }
else {
  this.onCreate();
}
   
  }
  onCreate(): void { if (this.productForm.invalid) {
    Swal.fire({
      icon: 'warning',
      title: 'Champs requis',
      text: 'Veuillez remplir tous les champs correctement.',
    });
    return;
  }

  const dto = this.productForm.value;
  const productData: CreateProductRequestDto = { dto };

  Swal.fire({
    title: 'Ajout du produit...',
    allowOutsideClick: false,
    allowEscapeKey: false,
    showConfirmButton: false,
    didOpen: () => Swal.showLoading(),
  });

  this.addUpdateService.addProduct(productData).subscribe({
    next: () => {
      Swal.fire({
        icon: 'success',
        title: 'Succès',
        text: 'Produit ajouté avec succès !',
        confirmButtonText: 'OK',
      }).then(() => this.router.navigate(['/produits']));
    },
    error: (err) => {
      console.error('Erreur lors de l’ajout du produit:', err);
      Swal.fire({
        icon: 'error',
        title: 'Échec',
        text: 'Impossible d’ajouter le produit. Veuillez réessayer.',
      });
    },
  });}
}
