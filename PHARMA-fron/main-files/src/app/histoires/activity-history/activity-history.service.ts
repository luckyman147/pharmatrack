import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ActivityHistoryResponse } from './activity-log.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityHistoryService {
  private apiUrl = `${environment.apiUrl}/ActivityHistory`;

  constructor(private http: HttpClient) { }

  getActivityHistory(pageIndex: number, pageSize: number): Observable<ActivityHistoryResponse> {
    let params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    // The endpoint is a GET as per backend implementation
    return this.http.get<ActivityHistoryResponse>(this.apiUrl, { params });
  }
}
