import { Component, OnInit } from '@angular/core';
import { ProductDetailService } from './product-detail.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductDetail } from '../dto/ProductDetail';
import { AddUpdateProduitService } from '../add-update-produit/add-update-produit.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
})
export class ProductDetailComponent implements OnInit {
  produit: ProductDetail;
  constructor(
    private productDetailService: ProductDetailService,
    private router: Router,
    private route: ActivatedRoute, private addUpdateService: AddUpdateProduitService
  ) {}
  goBack(): void {
    this.router.navigate(['/produits']);
  }

  editProduct(product: ProductDetail): void {
    this.router.navigate([`/produits/edit/${product.id}`], {
      state: { productData: product, isUpdated: true }, // pass product data to edit page
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
        this.router.navigate(['/produits']);
      },
      error: (err) => {
        console.error('Error deleting product:', err);
      },
    });
  }

  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.productDetailService.getProductById(productId).subscribe(
        (product) => {
          this.produit = product.produit as ProductDetail;
          // Handle the product data
        },
        (error) => {
          console.error('Error fetching product details:', error);

          // Handle error
        }
      );
    }
  }
}
