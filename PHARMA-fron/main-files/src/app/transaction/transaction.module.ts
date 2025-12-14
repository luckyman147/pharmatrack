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
import { PharmacieFilterComponent } from './pharmacies/pharmacie-filter/pharmacie-filter.component';
import { PharmacieService } from './pharmacies/pharmacie.service';
import { VisiteStateService } from './create-update-viste/visite-state.service';
import { CreatePharmacieComponent } from './pharmacies/create-pharmacie/create-pharmacie.component';

@NgModule({
  declarations: [
    OrderComponent,
    VisiteComponent,
    CreateUpdateVisteComponent,
    ComponentsComponent,
    PharmaciesComponent,
    PharmacieFilterComponent,
    CreatePharmacieComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    TransactionRoutingModule,
  ],
  providers: [VisiteService, PharmacieService, VisiteStateService],
})
export class TransactionModule {}
