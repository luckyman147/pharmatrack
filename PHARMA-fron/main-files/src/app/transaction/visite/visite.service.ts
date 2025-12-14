import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { PaginatedResult } from './models/PaginatedResult';
import { Observable } from 'rxjs/internal/Observable';
import { GroupedResult } from './models/GroupedResult';
import { User } from 'src/app/shared/Models/User';
import { UpdateVisteDto } from './DTO/updateViisteDto';

@Injectable({
  providedIn: 'root'
})
export class VisiteService {

  private visiteUrl: string;

  constructor(  private http: HttpClient,
      private appConfigService: AppConfigService) {

        this.visiteUrl = `${this.appConfigService.apiUrl}/visites`;
       }
//? Delete Visite 
  deleteVisite(id: string): Observable<void> {
    console.log('Deleting visite with id:', id);
    return this.http.delete<void>(`${this.visiteUrl}/${id}`);
  }
  //? Get Visites with pagination and filters

       getVisites( pageIndex: number,
    pageSize: number=12,startDate: string | null, endDate: string | null, byName: string | null): Observable<PaginatedResult<GroupedResult>> {

  console.log('VisiteService#getVisites called with params:', { pageIndex, pageSize, startDate, endDate, byName });
      const delegueId: User = JSON.parse(localStorage.getItem('user') || '{}');
      if (!delegueId || !delegueId.id) {
        throw new Error('Delegue ID is not available in local storage.');
      }


console.log('Fetching visites for delegueId:', delegueId.id, 'with params:', { pageIndex, pageSize, startDate, endDate,byName });
      const body = {
        delegueId: delegueId.id,
        startDate: startDate,
        endDate: endDate,
        pageIndex: pageIndex,
        pageSize: pageSize,
        byName: byName,
        groupedByPharmacie: null,
        filtredByReportUrl: null,
      };

 return this.http.post<PaginatedResult<any>>(
   this.visiteUrl + `/delegue?PageIndex=${pageIndex}&PageSize=${pageSize}`,
   body
 );
  }
  //! UpdateVisite
  updateVisite(visiteDate:UpdateVisteDto): Observable<any>{
    return this.http.put(this.visiteUrl,visiteDate)
  }

  //* Create Visite
  createVisite(visiteData: any): Observable<any> {
    return this.http.post<any>(this.visiteUrl, visiteData);
  }
}
