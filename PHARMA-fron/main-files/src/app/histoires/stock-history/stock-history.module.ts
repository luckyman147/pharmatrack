import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { StockHistoryComponent } from './stock-history.component';
import { StockHistoryRoutingModule } from './stock-history-routing.module';

const routes: Routes = [
  { path: '', component: StockHistoryComponent }
];

@NgModule({
  declarations: [
    StockHistoryComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    StockHistoryRoutingModule,
    FormsModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    StockHistoryComponent
  ]
})
export class StockHistoryModule { }
