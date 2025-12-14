import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ProductFilterRequest } from '../dto/ProductFilterRequest';
import { forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AllProductsService } from './all-products.service';
import { AllCategoriesService } from '../categorie/all-categories/all-categories.service';
import { Category } from '../categorie/models/Categorie';
import { Product } from '../models/Product';
import { PaginatedProductResult } from '../dto/PaginationProductResult';
import { th } from 'date-fns/locale';
import { AddUpdateProduitService } from '../add-update-produit/add-update-produit.service';

@Component({
  selector: 'app-all-products',
  templateUrl: './all-products.component.html',
  styleUrls: ['./all-products.component.scss'],
})
export class AllProductsComponent implements OnInit {
  products: Product[] = [];
  isLoading: boolean = false;

  // Pagination State
  paginationResult: PaginatedProductResult = {
    pageIndex: 1,
    pageSize: 13,
    count: 0,
    pageReste: 0,
    data: [],
  };

  // Filter Model (Bound to HTML inputs)
  filter: ProductFilterRequest['filter'] = {
    categorieId: null, // Set default to 'all' for the dropdown
    name: null,
    categorieName: null,
    minPrixVente: null,
    maxPrixVente: null,
  };

  // UI State for Grid/List Toggle
  viewMode: 'list' | 'grid' = 'list';

  // Categories for the filter dropdown
  availableCategories: Category[] = [{ id: 'all', name: 'All category' }];

  // --- Utility Getters ---

  get totalPages(): number {
    if (
      this.paginationResult.count === 0 ||
      this.paginationResult.pageSize === 0
    )
      return 0;
    return Math.ceil(
      this.paginationResult.count / this.paginationResult.pageSize
    );
  }

  get pageNumbersArray(): number[] {
    // Generates an array of page indices for the pagination links [0, 1, 2, ...]
    return Array.from({ length: this.totalPages }, (_, i) => i);
  }

  // --- Constructor and Initialization ---

  constructor(
    private productService: AllProductsService,
    private categoryService: AllCategoriesService,
    private route: ActivatedRoute,
    private addUpdateService: AddUpdateProduitService,
    private router: Router // Inject Router for navigation
  ) {}

  ngOnInit(): void {
    // Load necessary initial data (Categories and the first page of Products)
    this.loadInitialData();
  }

  /**
   * Loads the list of categories and the initial product list (page 0).
   */
  loadInitialData(): void {
    // 1. Fetch Categories
    this.categoryService.getCategories(this.filter.categorieName).subscribe({
      next: (categories) => {
        // Clear and rebuild categories list, ensuring 'All category' is first
        this.availableCategories = [
          { id: 'all', name: 'All category' },
          ...categories,
        ];

        // Set the filter default to 'all' for the dropdown
        this.filter.categorieId = 'all';

        // 2. Fetch Initial Products (sequentially after categories are loaded)
        // Note: Using the current pageIndex, which is 1 initially.
        this.fetchProducts(this.paginationResult.pageIndex);
      },
      error: (err) => {
        console.error('Error loading Categories:', err);
        // If categories fail, still attempt to load products without category filter
        this.fetchProducts(this.paginationResult.pageIndex);
        // TODO: Add notification for category load failure
      },
    });
  }
  deleteProduct(productId: string) {
    // Show confirmation dialog before deletion
    const confirmed = window.confirm(
      'Are you sure you want to delete this product? This action cannot be undone.'
    );
    if (!confirmed) {
      return;
    }
    // Implement the delete functionality here
    this.addUpdateService.deleteProduct(productId).subscribe({
      next: () => {
        console.log(`Product with ID ${productId} deleted successfully.`);
        // Refresh the product list after deletion
        this.fetchProducts(this.paginationResult.pageIndex);
      },
      error: (err) => {
        console.error('Error deleting product:', err);
      },
    });
  }
  // --- Data Fetching and Filtering ---
  fetchProducts(pageNumber: number): void {
    if (this.isLoading) return;

    this.isLoading = true;
    this.paginationResult.pageIndex = pageNumber; // Update page index immediately

    const selectedCategory = this.availableCategories.find(
      (c) => c.id === this.filter.categorieId
    );

    // 1. Construct the API Request Payload based on filter state
    const payload: ProductFilterRequest = {
      filter: {
        // Spread the current filter object
        ...this.filter,

        // Conditional logic for category filtering:
        categorieId:
          this.filter.categorieId === 'all' ? null : this.filter.categorieId,
        categorieName:
          selectedCategory && selectedCategory.id !== 'all'
            ? selectedCategory.name
            : null,

        // Ensure price fields are sent as null if they are 0 or empty, adhering to API spec
        minPrixVente:
          this.filter.minPrixVente > 0 ? this.filter.minPrixVente : null,
        maxPrixVente:
          this.filter.maxPrixVente > 0 ? this.filter.maxPrixVente : null,

        // Ensure name is null if empty string
        name: this.filter.name ? this.filter.name.trim() : null,
      },
      pageNumber: pageNumber,
      pageSize: this.paginationResult.pageSize,
    };

    console.log('API Request Payload:', payload);

    // 2. Call the service and manage loading state
    this.productService
      .getProducts(payload)
      .pipe(
        // Ensure loading state is turned off when the stream completes or errors
        finalize(() => (this.isLoading = false))
      )
      .subscribe({
        next: (result) => {
          console.log('API Response:', result);
          // Update component state with the response data
          this.paginationResult = result.paginationResult;
          this.products = result.paginationResult.data || [];
        },
        error: (err) => {
          console.error('Error fetching products:', err);
          this.products = [];
          // Reset pagination data on failure
          this.paginationResult = {
            ...this.paginationResult,
            count: 0,
            pageReste: 0,
            pageIndex: 0,
          };
          // TODO: Add a notification service call here for user feedback
        },
      });
  }

  // --- Navigation ---

  /**
   * Navigates to a specific page number and triggers a data fetch.
   * @param page The page index (0-based)
   */

  goToPage(page: number): void {
    if (page >= 0 && page < this.totalPages) {
      this.paginationResult.pageIndex = page;
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { page: this.paginationResult.pageIndex },
        queryParamsHandling: 'merge',
      });
      this.fetchProducts(this.paginationResult.pageIndex);
    }
  }

  nextPage(): void {
    if (this.paginationResult.pageIndex < this.totalPages - 1) {
      this.paginationResult.pageIndex++;
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { page: this.paginationResult.pageIndex },
        queryParamsHandling: 'merge',
      });
      this.fetchProducts(this.paginationResult.pageIndex);
    }
  }

  prevPage(): void {
    if (this.paginationResult.pageIndex > 0) {
      this.paginationResult.pageIndex--;
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { page: this.paginationResult.pageIndex - 1 },
        queryParamsHandling: 'merge',
      });
      this.fetchProducts(this.paginationResult.pageIndex);
    }
  }
  editProduct(product: Product) {
    this.router.navigate([`/produits/edit/${product.id}`], {
      state: { productData: product, isUpdated: true }, // pass product data to edit page
    });
  }
  /**
   * Handles navigation to the product creation page using Angular Router.
   */
  goToCreateProductPage(): void {
    // Assuming the route to create a new product is '/ecommerce/products/create'
    this.router
      .navigate(['/produits/add'], {
        state: { categories: this.availableCategories },
      })
      .catch((err) => {
        console.error('Navigation failed:', err);
      });
  }
}
