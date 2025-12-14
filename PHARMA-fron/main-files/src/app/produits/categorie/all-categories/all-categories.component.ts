import { Component, OnInit } from '@angular/core';
import { Category } from '../models/Categorie';
import { AllCategoriesService } from './all-categories.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-all-categories',
  templateUrl: './all-categories.component.html',
  styleUrls: ['./all-categories.component.scss'],
})
export class AllCategoriesComponent implements OnInit {
  categories: Category[] = [];
 categoryId?: string;
isUpdateMode: boolean = false;
 categoryName?: string;
  searchName: string = '';
  isLoading = false;
  constructor(private allCategoriesService: AllCategoriesService,private router: Router) {}

  ngOnInit(): void {
    this.loadCategories();
  }
  onSearch(): void {
    this.loadCategories();
  }
openAddCategory(): void {
  this.router.navigate(['/categorie/new']);
  // Logic to open add category modal or navigate to add category page
  
    // Logic to open add category modal or navigate to add category page
  }

  updateCategory(category: Category): void {
    this.categoryId = category.id;
    this.isUpdateMode = true;
    this.categoryName = category.name;
  }

  deleteCategory(id: string): void {
    this.allCategoriesService.deleteCategory(id).subscribe({
      next: () => {
        this.categories = this.categories.filter(cat => cat.id !== id);
      },
      error: (error) => {
        console.error('Error deleting category:', error);
      }
    });
  }
  loadCategories(): void {
      this.isLoading = true;
    this.allCategoriesService.getCategories(this.searchName).subscribe(
      (categories) => {
        this.isLoading = false;
        this.categories = categories;
      },
      (error) => {
        console.error('Error loading categories:', error);
        this.isLoading = false;
      }
    );
  }
}
