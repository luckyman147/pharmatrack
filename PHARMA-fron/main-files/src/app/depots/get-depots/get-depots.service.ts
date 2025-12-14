import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { Depot } from '../models/depot';
import { DepotsFilterDto } from '../dto/depotsFilterDto.dtos';
import { PaginatedResult } from 'src/app/transaction/visite/models/PaginatedResult';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetDepotsService {

  constructor(private http: HttpClient, private appConfigService: AppConfigService  ) {
    this.depotsUrl = `${this.appConfigService.apiUrl}/Depots`;
   }
  private depotsUrl!: string;
  getDepots(pageIndex: number, pageSize: number, depotFilter?: DepotsFilterDto): Observable<any> {
    return this.http.post<any >(this.depotsUrl + `?pageIndex=${pageIndex}&pageSize=${pageSize}`, { ...depotFilter });
  }

  getDepotsStockForProduct(produitId: string): Observable<any> {
    return this.http.get<any>(`${this.appConfigService.apiUrl}/Depots/product/${produitId}`);
  }
}