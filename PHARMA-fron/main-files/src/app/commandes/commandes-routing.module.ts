import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommandeListComponent } from './commande-list/commande-list.component';
import { CommandeDetailComponent } from './commande-detail/commande-detail.component';
import { AddUpdateCommandeComponent } from './add-update-commande/add-update-commande.component';

const routes: Routes = [
  {
    path: '',
    component: CommandeListComponent,
    data: {
      title: 'Liste des Commandes'
    }
  },
  {
    path: 'new',
    component: AddUpdateCommandeComponent,
    data: {
      title: 'Nouvelle Commande'
    }
  },
  {
    path: ':id/edit',
    component: AddUpdateCommandeComponent,
    data: {
      title: 'Modifier Commande'
    }
  },
  {
    path: ':id',
    component: CommandeDetailComponent,
    data: {
      title: 'Détail de Commande'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommandesRoutingModule { }
