import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GetDepotsComponent } from './get-depots/get-depots.component';
import { AddUpdateDepotComponent } from './add-update-depot/add-update-depot.component';
import { DepotDetailComponent } from './depot-detail/depot-detail.component';

const routes: Routes = [
  {
    path: '',
    component: GetDepotsComponent
  }
  , {
    path: 'add',
component:AddUpdateDepotComponent  }, 
  {
    path: ':id',
component:DepotDetailComponent
  },
  {
    path: 'edit/:id',
component:AddUpdateDepotComponent
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DepotsRoutingModule { }
