import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommandeListComponent } from './components/commande-list/commande-list.component';
import { CommandeDetailComponent } from './components/commande-detail/commande-detail.component';

const routes: Routes = [
  {
    path: '',
    component: CommandeListComponent
  },
  {
    path: ':id',
    component: CommandeDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommandesRoutingModule { }
