import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategorieRoutingModule } from './categorie-routing.module';
import { AllCategoriesComponent } from './all-categories/all-categories.component';
import { AddUpdateCategorieComponent } from './add-update-categorie/add-update-categorie.component';
import { AllCategoriesService } from './all-categories/all-categories.service';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AllCategoriesComponent,
    AddUpdateCategorieComponent,
    
  ],
  imports: [
    FormsModule,
    CommonModule,
    CategorieRoutingModule
  ],
  providers: [AllCategoriesService]
})
export class CategorieModule { }
