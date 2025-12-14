import { Component, OnInit } from '@angular/core';
import { StockHistoryService } from './stock-history.service';
import { StockTransactionHistory, StockHistoryFilter, TransactionReasonLabel } from './stock-transaction.model';

@Component({
  selector: 'app-stock-history',
  templateUrl: './stock-history.component.html',
  styleUrls: []
})
export class StockHistoryComponent implements OnInit {
  history: StockTransactionHistory[] = [];
  loading = false;
  pageIndex = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  filter: StockHistoryFilter = {};

  constructor(private stockService: StockHistoryService) { }

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory(): void {
    this.loading = true;
    this.stockService.getStockHistory(this.pageIndex, this.pageSize, this.filter).subscribe({
      next: (response) => {
        this.history = response.result.data;
        this.totalCount = response.result.count;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading stock history', err);
        this.loading = false;
      }
    });
  }

  applyFilter(): void {
    this.pageIndex = 1;
    this.loadHistory();
  }

  resetFilter(): void {
    this.filter = {};
    this.pageIndex = 1;
    this.loadHistory();
  }

  onPageChange(newPage: number): void {
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.pageIndex = newPage;
      this.loadHistory();
    }
  }

  getReasonLabel(reason: number): string {
    return TransactionReasonLabel[reason] || 'Inconnu';
  }
}
