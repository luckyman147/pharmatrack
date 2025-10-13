import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { PaginatedResult } from './models/PaginatedResult';
import { Observable } from 'rxjs/internal/Observable';
import { GroupedResult } from './models/GroupedResult';
import { User } from 'src/app/shared/Models/User';

@Injectable({
  providedIn: 'root'
})
export class VisiteService {

  private visiteUrl: string;

  constructor(  private http: HttpClient,
      private appConfigService: AppConfigService) {

        this.visiteUrl = `${this.appConfigService.apiUrl}/visites`;
       }

  deleteVisite(id: string): Observable<void> {
    console.log('Deleting visite with id:', id);
    return this.http.delete<void>(`${this.visiteUrl}/${id}`);
  }

       getVisites( pageIndex: number,
    pageSize: number=12,startDate: string | null, endDate: string | null): Observable<PaginatedResult<GroupedResult>> {
      
  console.log('VisiteService#getVisites called with params:', { pageIndex, pageSize, startDate, endDate });
      const delegueId: User = JSON.parse(localStorage.getItem('user') || '{}');
      if (!delegueId || !delegueId.id) {
        throw new Error('Delegue ID is not available in local storage.');
      }


console.log('Fetching visites for delegueId:', delegueId.id, 'with params:', { pageIndex, pageSize, startDate, endDate });
      const body = {
        delegueId: "a0087472-51cc-4a68-b43e-46a0a8f0beda",
        startDate: startDate,
        endDate: endDate,
        pageIndex: pageIndex,
        pageSize: pageSize,
        groupedByPharmacie: null,
        filtredByReportUrl: null,
      };

 return this.http.post<PaginatedResult<any>>(
   this.visiteUrl + `/delegue?PageIndex=${pageIndex}&PageSize=${pageSize}`,
   body
 );
  }
}
