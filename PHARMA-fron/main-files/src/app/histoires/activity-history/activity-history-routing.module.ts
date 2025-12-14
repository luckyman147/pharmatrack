import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityHistoryComponent } from './activity-history.component';
const routes: Routes = [
  {
    path: '',
    component: ActivityHistoryComponent,
    data: {
      title: 'Activity History'
    }
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ActivityHistoryRoutingModule { }