import { Component, OnInit } from '@angular/core';
import { VisiteService } from './visite.service';
import { GroupedResult } from './models/GroupedResult';
import { PaginatedResult } from './models/PaginatedResult';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Visite } from './models/Visite';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-visite',
  templateUrl: './visite.component.html',
  styleUrls: ['./visite.component.scss'],
})
export class VisiteComponent implements OnInit {
  visites$!: Observable<GroupedResult[]>; // 👈 Used with async pipe

  filterForm!: FormGroup;
  pageIndex: number = 0;
  pageSize: number = 5;
  totalCount: number = 0;
  totalPages: number = 0;
  pageNumbers: number[] = [];

  constructor(
    private visiteService: VisiteService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      startDate: [null],
      endDate: [null],
      byName: [''],
    });

    this.route.queryParams.subscribe((params) => {
      this.filterForm.patchValue({
        startDate: params['startDate'] || null,
        endDate: params['endDate'] || null,
        byName: params['byName'] || '',
      });

      this.pageIndex = +params['page'] || 0;
      this.loadDefaultVisites();
    });
  }
  applyFilters(): void {
    const { startDate, endDate, byName } = this.filterForm.value;
    this.pageIndex = 0;

    // Update the URL with new filters
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        startDate: startDate || null,
        endDate: endDate || null,
        byName: byName || null,
        page: this.pageIndex,
      },
      queryParamsHandling: 'merge', // keep existing params like pagination
    });

    this.loadDefaultVisites();
  }
  deleteVisite(visite: Visite): void {
    this.visiteService.deleteVisite(visite.id).subscribe(() => {
      this.loadDefaultVisites();
    });
  }
  loadDefaultVisites(): void {
    const startDate = this.filterForm.value.startDate;
    const endDate = this.filterForm.value.endDate;
    const byName = this.filterForm.value.byName;
    this.visites$ = this.visiteService
      .getVisites(this.pageIndex, this.pageSize, startDate, endDate, byName)
      .pipe(
        map((result: any) => {
          const visites = result.visites ?? result;

          // 👇 Extract pagination data
          this.totalCount = visites.count;
          this.pageSize = visites.pageSize;
          this.pageIndex = visites.pageIndex;
          this.totalPages = Math.ceil(visites.count / visites.pageSize);

          // 👇 Generate page numbers
          this.updatePageNumbers(); // ✅ call helper for smart pagination

          console.log('Paginated Result:', visites);
          return visites.data ?? [];
        })
      );
  }
  NavigateToCreateVisite(): void {
    this.router.navigate(['Transaction/Creer-Visite']);
  }
  private updatePageNumbers(): void {
    const maxVisible = 5; // how many page numbers to show
    const half = Math.floor(maxVisible / 2);

    let start = Math.max(0, this.pageIndex - half);
    let end = Math.min(this.totalPages - 1, this.pageIndex + half);

    // Adjust if near the start
    if (this.pageIndex < half) {
      end = Math.min(this.totalPages - 1, maxVisible - 1);
    }

    // Adjust if near the end
    if (this.pageIndex + half >= this.totalPages) {
      start = Math.max(0, this.totalPages - maxVisible);
    }

    this.pageNumbers = [];
    for (let i = start; i <= end; i++) {
      this.pageNumbers.push(i);
    }
  }

  goToPage(page: number): void {
    if (page >= 0 && page < this.totalPages) {
      this.pageIndex = page;
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { page: this.pageIndex },
        queryParamsHandling: 'merge',
      });
      this.loadDefaultVisites();
    }
  }

  nextPage(): void {
    if (this.pageIndex < this.totalPages - 1) {
      this.pageIndex++;
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { page: this.pageIndex },
        queryParamsHandling: 'merge',
      });
      this.loadDefaultVisites();
    }
  }

  prevPage(): void {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { page: this.pageIndex - 1 },
        queryParamsHandling: 'merge',
      });
      this.loadDefaultVisites();
    }
  }
}
