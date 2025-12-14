import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProduitsRoutingModule } from './produits-routing.module';
import { CategorieModule } from './categorie/categorie.module';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { AddUpdateProduitComponent } from './add-update-produit/add-update-produit.component';
import { AllProductsComponent } from './all-products/all-products.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductDetailService } from './product-detail/product-detail.service';

@NgModule({
  declarations: [
    AddUpdateProduitComponent,
    ProductDetailComponent,
    AddUpdateProduitComponent,
    AllProductsComponent,
  ],
  imports: [
    CommonModule,
    ProduitsRoutingModule,
    ReactiveFormsModule, // ✅ ADD THIS
    FormsModule,
  ],
  providers: [ProductDetailService],
})
export class ProduitsModule {}
