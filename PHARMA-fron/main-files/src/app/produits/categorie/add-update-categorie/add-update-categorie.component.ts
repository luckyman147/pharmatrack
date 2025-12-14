import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { AllCategoriesService } from '../all-categories/all-categories.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-update-categorie',
  templateUrl: './add-update-categorie.component.html',
  styleUrls: ['./add-update-categorie.component.scss'],
  inputs: ['categoryId', 'isUpdateMode', 'categoryName'],
  
})
export class AddUpdateCategorieComponent implements OnInit {
  @Input() categoryId?: string;
  @Input() isUpdateMode: boolean = false;
  @Input() categoryName?: string;

  @Output() LoadChanges = new EventEmitter<void>();
  
  categoryNameInput: string = '';
  constructor(private allCategoriesService: AllCategoriesService) {}
  // 👇 This ensures the input updates dynamically when parent changes it
  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['categoryName'] &&
      changes['categoryName'].currentValue !== undefined
    ) {
      this.categoryNameInput = changes['categoryName'].currentValue;
    }
  }
  ngOnInit(): void {
    console.log('Init categoryName:', this.categoryName);
    this.categoryNameInput = this.categoryName || '';  }

  addCategory() {
    const name = this.categoryNameInput.trim();
    if (!name) {
      Swal.fire({
        icon: 'warning',
        title: 'Missing name',
        text: 'Category name cannot be empty.',
      });
      return;
    }

    const CreateCateg = { name };

    Swal.fire({
      title: 'Adding category...',
      allowOutsideClick: false,
      allowEscapeKey: false,
      showConfirmButton: false,
      didOpen: () => Swal.showLoading(),
    });

    this.allCategoriesService.addCategory(CreateCateg).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Success',
          text: 'Category added successfully!',
        });
        this.categoryNameInput = '';
        this.categoryId = undefined;
        this.isUpdateMode = false;
        this.categoryName = '';
        this.LoadChanges.emit();
      },
      error: (error) => {
        console.error('Error adding category:', error);
        Swal.fire({
          icon: 'error',
          title: 'Failed',
          text: 'Failed to add category. Please try again.',
        });
      },
    });
  }
  updateCategory() {
    if (!this.categoryId) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Category ID is missing for update.',
      });
      return;
    }

    const name = this.categoryNameInput.trim();
    if (!name) {
      Swal.fire({
        icon: 'warning',
        title: 'Missing name',
        text: 'Category name cannot be empty.',
      });
      return;
    }

    const updatedCategory = { id: this.categoryId, name: this.categoryNameInput };
    Swal.fire({
      title: 'Updating category...',
      allowOutsideClick: false,
      allowEscapeKey: false,
      showConfirmButton: false,
      didOpen: () => Swal.showLoading(),
    });

    this.allCategoriesService
      .updateCategory( updatedCategory)
      .subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Category updated successfully!',
          });
          this.categoryNameInput = '';
          this.categoryId = undefined;
          this.isUpdateMode = false;
          this.categoryName = '';
          this.LoadChanges.emit();
        },
        error: (error) => {
          console.error('Error updating category:', error);
          Swal.fire({
            icon: 'error',
            title: 'Failed',
            text: 'Failed to update category. Please try again.',
          });
        },
      });
  }
}
