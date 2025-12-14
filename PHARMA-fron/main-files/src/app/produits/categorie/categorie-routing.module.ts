import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllCategoriesComponent } from './all-categories/all-categories.component';
import { AddUpdateCategorieComponent } from './add-update-categorie/add-update-categorie.component';

const routes: Routes = [
  {
    path: '',
    component: AllCategoriesComponent,
    data :{
      title:"Tous Les categories"
    }
  },
  {
    path: 'Update/:id',
    component: AddUpdateCategorieComponent,

  }
  ,
  {
    path: 'new',
    component: AddUpdateCategorieComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CategorieRoutingModule { }
