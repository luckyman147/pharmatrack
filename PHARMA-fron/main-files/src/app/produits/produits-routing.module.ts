import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllProductsComponent } from './all-products/all-products.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { AddUpdateProduitComponent } from './add-update-produit/add-update-produit.component';

const routes: Routes = [
  {
    path: '',
    component: AllProductsComponent,
    data: {
      title: 'All Products'
    }
  },
  // {
  //   path: ':id',
  //   component:ProductDetailComponent,
  //   data: {
  //     title: ' Detail de Produit'
  //   },
  // },
  {
    path: 'add',
    component: AddUpdateProduitComponent,
    data: {
      title: 'Ajouter un Produit'
    }
  },
  {
    path: 'edit/:id',
    component: AddUpdateProduitComponent,
    data: {
      title: 'Modifier un Produit'
    }
  },
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProduitsRoutingModule { }
