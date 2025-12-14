import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'Stock-History',
   loadChildren: () => import('./stock-history/stock-history.module').then(m => m.StockHistoryModule)
  },
  {
    path: 'activites-History',
    loadChildren: () => import('./activity-history/activity-history.module').then(m => m.ActivityHistoryModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HistoiresRoutingModule { }
