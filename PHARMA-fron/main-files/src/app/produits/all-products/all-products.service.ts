import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { ProductFilterRequest } from '../dto/ProductFilterRequest';
import { Observable } from 'rxjs';
import { PaginatedProductResult } from '../dto/PaginationProductResult';

@Injectable({
  providedIn: 'root'
})
export class AllProductsService {

  constructor(private http: HttpClient, 
 private appConfigService: AppConfigService  ) {
    this.productsUrl = `${this.appConfigService.apiUrl}`;
  }
  private productsUrl!: string;
getProducts(Filter?: ProductFilterRequest) : Observable<any> {
    return this.http.post<any>(this.productsUrl + `/Getproducts?pageIndex=${Filter?.pageNumber}&pageSize=${Filter?.pageSize}` , Filter);
  }
}
