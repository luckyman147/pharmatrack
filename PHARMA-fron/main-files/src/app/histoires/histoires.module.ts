import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HistoiresRoutingModule } from './histoires-routing.module';
import { StockHistoryModule } from './stock-history/stock-history.module';
import { ActivityHistoryModule } from './activity-history/activity-history.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HistoiresRoutingModule,
    StockHistoryModule,
    ActivityHistoryModule
  ]
})
export class HistoiresModule { }
