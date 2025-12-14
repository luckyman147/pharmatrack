import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { Category } from '../models/Categorie';
import { CreateCategoryDto } from '../dto/CreateCategorie.dto';

@Injectable({
  providedIn: 'root'
})
export class AllCategoriesService {

  constructor(private http: HttpClient,
        private appConfigService: AppConfigService
    
  ) { 
    this.categoriesUrl = `${this.appConfigService.apiUrl}/Category`;
  }
  private categoriesUrl!: string;
  
  getCategories(name?: string): Observable<Category[]> {
    let params = new HttpParams();
    if (name) {
      params = params.set('name', name);
    }
    return this.http.get<Category[]>(this.categoriesUrl, { params });
  }

  addCategory(dto: CreateCategoryDto): Observable<Category> {
    return this.http.post<Category>(this.categoriesUrl, dto);
  }

  updateCategory( category: Partial<Category>): Observable<Category> {
    return this.http.put<Category>(`${this.categoriesUrl}`, category);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.categoriesUrl}/${id}`);
  }

}
