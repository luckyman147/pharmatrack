import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { CreateProductRequestDto } from '../dto/CreateProductRequestDto';
import { Observable } from 'rxjs';
import { UpdateProductRequestDto } from '../dto/UpdateProductRequestDTo';

@Injectable({
  providedIn: 'root',
})
export class AddUpdateProduitService {
  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) {}

  addProduct(productData: CreateProductRequestDto): Observable<any> {
    const apiUrl = this.appConfigService.apiUrl + '/products';
    return this.http.post(apiUrl, productData);
  }
  updateProduct(
    productId: string,
    productData: UpdateProductRequestDto
  ): Observable<any> {
    const apiUrl = this.appConfigService.apiUrl + `/products/${productId}`;
    return this.http.put(apiUrl, productData);
  }
  deleteProduct(productId: string): Observable<any> {
    const apiUrl = this.appConfigService.apiUrl + `/products/${productId}`;
    return this.http.delete(apiUrl);
  }
}
