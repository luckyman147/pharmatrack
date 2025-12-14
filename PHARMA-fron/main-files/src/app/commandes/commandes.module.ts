import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { CommandesRoutingModule } from './commandes-routing.module';
import { CommandeListComponent } from './commande-list/commande-list.component';
import { CommandeDetailComponent } from './commande-detail/commande-detail.component';
import { CommandeService } from './commande.service';
import { AddUpdateCommandeComponent } from './add-update-commande/add-update-commande.component';

@NgModule({
  declarations: [
    CommandeListComponent,
    CommandeDetailComponent,
    AddUpdateCommandeComponent
  ],
  imports: [
    CommonModule,
    CommandesRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [CommandeService]
})
export class CommandesModule { }
