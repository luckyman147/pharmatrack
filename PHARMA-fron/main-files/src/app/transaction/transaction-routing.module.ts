import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VisiteComponent } from './visite/visite.component';
import { OrderComponent } from './order/order.component';
import { PharmaciesComponent } from './pharmacies/pharmacies.component';


const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'Visites',
        component: VisiteComponent,
        data: {
          title: 'Visites'
        }
      },
      {
        path: 'Commandes',
        component: OrderComponent,
        data: {
          title: 'Commandes'
        }
      },
 
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionRoutingModule { }
