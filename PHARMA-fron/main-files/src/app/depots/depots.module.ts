import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DepotsRoutingModule } from './depots-routing.module';
import { GetDepotsComponent } from './get-depots/get-depots.component';
import { AddUpdateDepotComponent } from './add-update-depot/add-update-depot.component';
import { DepotDetailComponent } from './depot-detail/depot-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FilterDepotsComponent } from './filter-depots/filter-depots.component';


@NgModule({
  declarations: [
    GetDepotsComponent,
    AddUpdateDepotComponent,
    DepotDetailComponent,
    FilterDepotsComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DepotsRoutingModule
  ]
})
export class DepotsModule { }
