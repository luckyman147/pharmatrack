import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VisiteComponent } from './visite/visite.component';
import { OrderComponent } from './order/order.component';
import { PharmaciesComponent } from './pharmacies/pharmacies.component';
import { CreateUpdateVisteComponent } from './create-update-viste/create-update-viste.component';


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
  path: 'Creer-Visite',
  component: CreateUpdateVisteComponent,
  data: { title: 'Créer une Visite' },
},
{
  path: 'Modifier-Visite/:id',
  component: CreateUpdateVisteComponent,
  data: { title: 'Modifier une Visite' },
},

      {
        path: 'Commandes',
        loadChildren: () => import('../commandes/commandes.module').then(m => m.CommandesModule),
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
