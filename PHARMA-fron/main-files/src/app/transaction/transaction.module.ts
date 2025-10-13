import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TransactionRoutingModule } from './transaction-routing.module';
import { OrderComponent } from './order/order.component';
import { VisiteComponent } from './visite/visite.component';
import { CreateUpdateVisteComponent } from './create-update-viste/create-update-viste.component';
import { ComponentsComponent } from './visite/components/components.component';
import { VisiteService } from './visite/visite.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PharmaciesComponent } from './pharmacies/pharmacies.component';

@NgModule({
  declarations: [
    OrderComponent,
    VisiteComponent,
    CreateUpdateVisteComponent,
    ComponentsComponent,
    PharmaciesComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    TransactionRoutingModule
  ],providers: [VisiteService]

})
export class TransactionModule { }
