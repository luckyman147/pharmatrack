import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ZONES } from 'src/app/transaction/pharmacies/data/zoneData';
import { Depot } from '../models/depot';
import { GetDepotsService } from './get-depots.service';
import { DepotsFilterDto } from '../dto/depotsFilterDto.dtos';
import { Router } from '@angular/router';

@Component({
  selector: 'app-get-depots',
  templateUrl: './get-depots.component.html',
  styleUrls: ['./get-depots.component.scss'],
})
export class GetDepotsComponent implements OnInit {
  depots: Depot[] = [];
  form!: any;
  pageIndex = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  pageNumbers: number[] = [];

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private depotService: GetDepotsService
  ) {}
  onNewDepot(): void {
    this.router.navigate(['/depot/add']);
  }
  ngOnInit(): void {
this.fetchDepots();

  }
  applyFilter(filter:any): void {
    console.log('Filter LLLL:', filter);
    this.form=filter;
    this.pageSize=3;
    this.pageIndex=1;
    this.fetchDepots();
    // You can now call your service with this.form.value to filter depots
  }
  fetchDepots(): void {
    let DepotsFilterDto: DepotsFilterDto;
    if (!this.form) {
     DepotsFilterDto = {
        name: null,
        telephone: null,
        zone: null,
        gouvernorat: null,
        delegation: null,
      };
    } else {
      const forms = this.form;
     
      // log
      console.log('Form values before processing:', forms);
      if (forms.byName) {
        const isNumber = /^[0-9]+$/.test(forms.byName);
    const isAlpha = /^[a-zA-Z]+$/.test(forms.byName);
    if (forms.byName && isNumber && !isAlpha) {
      const telephone = forms.byName;
      forms.byName = null;
      forms.telephone = telephone;
    }}

      DepotsFilterDto = {
        name: forms.byName || null,
        telephone: forms.telephone || null,
        zone: forms.zone || null,
        gouvernorat: forms.gouvernerat || null,
        delegation: forms.delegation || null,
      };
}
    console.log('DepotsFilterDto being sent to service:', DepotsFilterDto);

    this.depotService
      .getDepots(this.pageIndex, this.pageSize, DepotsFilterDto)
      .subscribe(
        (response) => {
          console.log('Service response:', response);
          this.depots = response.depots.data;
          if (this.depots.length === 0) {
            console.log('No depots found with the given filter.');
            this.depots = [];
          }
          this.totalCount = response.depots.count;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          this.pageIndex = response.depots.pageIndex;
          this.pageSize = response.depots.pageSize;
          this.updatePageNumbers();
          console.log('Fetched depots:', this.depots);
        },
        (error) => {
          console.error('Error fetching depots:', error);
        }
      );
  }
    goToPage(page: number) {
    if (page >= 1 && page < this.totalPages) {
      this.pageIndex = page;
      this.fetchDepots();
    }
  }

  nextPage() {
    if (this.pageIndex < this.totalPages ) {
      this.pageIndex++;
      this.fetchDepots();
    }
  }

  prevPage() {
    if (this.pageIndex > 1) {
      this.pageIndex--;
      this.fetchDepots();
    }}
      private updatePageNumbers() {
    const maxVisible = 5;
    const half = Math.floor(maxVisible / 2);

    let start = Math.max(0, this.pageIndex - half);
    let end = Math.min(this.totalPages - 1, this.pageIndex + half);

    if (this.pageIndex < half) {
      end = Math.min(this.totalPages - 1, maxVisible - 1);
    }

    if (this.pageIndex + half >= this.totalPages) {
      start = Math.max(0, this.totalPages - maxVisible);
    }

    this.pageNumbers = [];
    for (let i = start; i <= end; i++) {
      this.pageNumbers.push(i);
    }
  }

  onNavigateToDetails(depot: Depot): void {
    // Navigate to depot details page
    // Assuming the route is 'depot-detail/:id' or similar. Checking route config would be ideal but I will follow standard.
    // User requested "get depot page... navigate to depot details page"
    this.router.navigate(['/depot', depot.id]);
  }

}
