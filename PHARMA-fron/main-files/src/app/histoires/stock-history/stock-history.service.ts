import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { StockHistoryFilter, StockHistoryResponse } from './stock-transaction.model';

@Injectable({
  providedIn: 'root'
})
export class StockHistoryService {
  private apiUrl = `${environment.apiUrl}/StockHistory`;

  constructor(private http: HttpClient) { }

  getStockHistory(pageIndex: number, pageSize: number, filter?: StockHistoryFilter): Observable<StockHistoryResponse> {
    let params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    // The endpoint is a POST with filter in body
    return this.http.post<StockHistoryResponse>(this.apiUrl, filter || {}, { params });
  }
}
