import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { ProductDetail } from '../dto/ProductDetail';

@Injectable({
  providedIn: 'root',
})
export class ProductDetailService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private environment: AppConfigService
  ) {
    this.productUrl = `${this.environment.apiUrl}/produits`;
  }
  private productUrl!: string;

  getProductById(id: string):Observable<any> {
    return this.http.get<any>(this.productUrl + `/${id}`);
  }
}
