import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ActivityHistoryComponent } from './activity-history.component';
import { ActivityHistoryRoutingModule } from './activity-history-routing.module';

const routes: Routes = [
  { path: '', component: ActivityHistoryComponent }
];

@NgModule({
  declarations: [
    ActivityHistoryComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ActivityHistoryRoutingModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    ActivityHistoryComponent
  ]
})
export class ActivityHistoryModule { }
