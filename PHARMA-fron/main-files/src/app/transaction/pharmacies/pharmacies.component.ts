import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Pharmacie } from './models/Pharmacie';
import { PaginatedResult } from '../visite/models/PaginatedResult';
import { PharmacieService } from './pharmacie.service';
import { PharmacieFilterRequest } from './dto/PharmacieFilterRequest';

@Component({
  selector: 'app-pharmacies',
  templateUrl: './pharmacies.component.html',
  styleUrls: ['./pharmacies.component.scss'],
})
export class PharmaciesComponent implements OnInit {
  pharmacies: Pharmacie[] = [];
  pageIndex = 0;
  pageSize = 12;
  totalCount = 0;
  totalPages = 0;
  pageNumbers: number[] = [];
  currentFilter: any = {}; // store the last applied filter
  constructor(private pharmacieService: PharmacieService) {}
  @Output() pharmacieSelected = new EventEmitter<Pharmacie>();
  @Output() CreatePharmacie = new EventEmitter<void>();
  @Output() OutputPharmacie = new EventEmitter<void>();
  ngOnInit(): void {
    this.loadPharmacies();
  }
  onUpdate(id:string){
    
    console.log ('pressed');
    localStorage.setItem('PharmacieId',id);
    this.OutputPharmacie.emit();
  }
  createPharmcie() {
    console.log ('pressed');
    this.CreatePharmacie.emit();
  }

  onFilterApplied(filter: any) {
    this.currentFilter = filter;
    this.pageIndex = 0; // reset page
    this.loadPharmacies();
  }
  selectedPharmacie?: Pharmacie;
  selectPharmacie(p: Pharmacie) {
    this.selectedPharmacie = p;
    this.pharmacieSelected.emit(p); // ✅ emit to parent
  }

  loadPharmacies(): void {
    var gov = this.currentFilter.gouvernerat;
    if (gov) {
      this.currentFilter.gouvernerat =
        gov.charAt(0).toUpperCase() + gov.slice(1).toLowerCase();
    }
    const filterRequest: PharmacieFilterRequest = {
      pageIndex: this.pageIndex,
      pageSize: this.pageSize,
      ByName: this.currentFilter.byName || null,
      type: this.currentFilter.type || null,
      horaire: this.currentFilter.horaire || null,
      zone: this.currentFilter.zone || null,
      gouvernerat: this.currentFilter.gouvernerat || null,
      delegation: this.currentFilter.delegation || null,
    };
    this.pharmacieService
      .getPharmacies(filterRequest)
      .subscribe((result: PaginatedResult<Pharmacie>) => {
        this.pharmacies = result.data;
        this.pageIndex = result.pageIndex;
        this.pageSize = result.pageSize;
        this.totalCount = result.count;
        if (result.data.length === 0 && this.pageIndex > 0) {
          this.selectPharmacie(this.pharmacies[0]);
        }
        this.totalPages = Math.ceil(result.count / result.pageSize);
        this.updatePageNumbers();
      });
  }
  deletePharmacieById(id: string) {
    if (confirm('Voulez-vous vraiment supprimer cette pharmacie ?')) {
      this.pharmacieService.deletePharmacie(id).subscribe(() => {
        // Refresh the list after delete
        this.loadPharmacies();
      });
    }
  }
  goToPage(page: number) {
    if (page >= 0 && page < this.totalPages) {
      this.pageIndex = page;
      this.loadPharmacies();
    }
  }

  nextPage() {
    if (this.pageIndex < this.totalPages - 1) {
      this.pageIndex++;
      this.loadPharmacies();
    }
  }

  prevPage() {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.loadPharmacies();
    }
  }

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
}
