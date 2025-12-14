import { Component, OnInit } from '@angular/core';
import { ActivityHistoryService } from './activity-history.service';
import { ActivityLog } from './activity-log.model';

@Component({
  selector: 'app-activity-history',
  templateUrl: './activity-history.component.html',
  styleUrls: []
})
export class ActivityHistoryComponent implements OnInit {
  logs: ActivityLog[] = [];
  loading = false;
  pageIndex = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  constructor(private activityService: ActivityHistoryService) { }

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory(): void {
    this.loading = true;
    this.activityService.getActivityHistory(this.pageIndex, this.pageSize).subscribe({
      next: (response) => {
        this.logs = response.result.data;
        this.totalCount = response.result.count;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading activity history', err);
        this.loading = false;
      }
    });
  }

  onPageChange(newPage: number): void {
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.pageIndex = newPage;
      this.loadHistory();
    }
  }
}
