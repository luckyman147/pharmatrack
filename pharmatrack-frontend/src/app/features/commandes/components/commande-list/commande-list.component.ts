import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommandeService } from '../../services/commande.service';
import { CommandeSummaryInfo, StatusCommande, CommandeFilter } from '../../models/commande.models';
import { PaginationResult } from '../../../../shared/models/pagination.models';

@Component({
  selector: 'app-commande-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './commande-list.component.html',
  styleUrl: './commande-list.component.scss'
})
export class CommandeListComponent implements OnInit {
  commandes: CommandeSummaryInfo[] = [];
  loading = false;
  error: string | null = null;
  
  // Pagination
  pageIndex = 0;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  
  // For template access
  Math = Math;
  StatusCommande = StatusCommande;
  statusOptions = Object.values(StatusCommande);
  
  // Filter form
  filterForm!: FormGroup;
  
  constructor(
    private commandeService: CommandeService,
    private fb: FormBuilder,
    private router: Router
  ) {}
  
  ngOnInit(): void {
    this.initFilterForm();
    this.loadCommandes();
  }
  
  initFilterForm(): void {
    this.filterForm = this.fb.group({
      status: [''],
      startDate: [''],
      endDate: [''],
      minAmount: [''],
      maxAmount: [''],
      codeComm: ['']
    });
  }
  
  loadCommandes(): void {
    this.loading = true;
    this.error = null;
    
    const filter = this.buildFilter();
    
    this.commandeService.getAllCommandes(
      { pageIndex: this.pageIndex, pageSize: this.pageSize },
      filter
    ).subscribe({
      next: (result: PaginationResult<CommandeSummaryInfo>) => {
        this.commandes = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erreur lors du chargement des commandes';
        this.loading = false;
        console.error('Error loading commandes:', err);
      }
    });
  }
  
  buildFilter(): CommandeFilter | undefined {
    const formValue = this.filterForm.value;
    const filter: CommandeFilter = {};
    
    if (formValue.status) filter.status = formValue.status;
    if (formValue.startDate) filter.startDate = formValue.startDate;
    if (formValue.endDate) filter.endDate = formValue.endDate;
    if (formValue.minAmount) filter.minAmount = +formValue.minAmount;
    if (formValue.maxAmount) filter.maxAmount = +formValue.maxAmount;
    if (formValue.codeComm) filter.codeComm = +formValue.codeComm;
    
    return Object.keys(filter).length > 0 ? filter : undefined;
  }
  
  onFilterChange(): void {
    this.pageIndex = 0;  // Reset to first page
    this.loadCommandes();
  }
  
  clearFilters(): void {
    this.filterForm.reset();
    this.pageIndex = 0;
    this.loadCommandes();
  }
  
  // Pagination methods
  onPageChange(newPage: number): void {
    if (newPage >= 0 && newPage < this.totalPages) {
      this.pageIndex = newPage;
      this.loadCommandes();
    }
  }
  
  onPageSizeChange(newSize: number): void {
    this.pageSize = newSize;
    this.pageIndex = 0;  // Reset to first page
    this.loadCommandes();
  }
  
  // Navigation methods
  viewCommande(id: string): void {
    this.router.navigate(['/Home/commandes', id]);
  }
  
  editCommande(id: string): void {
    this.router.navigate(['/Home/commandes', id, 'edit']);
  }
  
  createCommande(): void {
    this.router.navigate(['/Home/commandes/new']);
  }
  
  deleteCommande(id: string, codeComm: number): void {
    if (confirm(`Êtes-vous sûr de vouloir supprimer la commande #${codeComm}?`)) {
      this.commandeService.deleteCommande(id).subscribe({
        next: () => {
          this.loadCommandes();  // Reload the list
        },
        error: (err) => {
          this.error = 'Erreur lors de la suppression de la commande';
          console.error('Error deleting commande:', err);
        }
      });
    }
  }
  
  // Utility methods
  getStatusBadgeClass(status: StatusCommande): string {
    switch (status) {
      case StatusCommande.Valide:
        return 'badge bg-success';
      case StatusCommande.EnAttente:
        return 'badge bg-warning';
      case StatusCommande.EnRetour:
        return 'badge bg-info';
      case StatusCommande.Annulée:
        return 'badge bg-danger';
      default:
        return 'badge bg-secondary';
    }
  }
  
  getStatusLabel(status: StatusCommande): string {
    switch (status) {
      case StatusCommande.Valide:
        return 'Validé';
      case StatusCommande.EnAttente:
        return 'En Attente';
      case StatusCommande.EnRetour:
        return 'En Retour';
      case StatusCommande.Annulée:
        return 'Annulée';
      default:
        return status;
    }
  }
  
  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('fr-FR');
  }
  
  formatCurrency(amount: number): string {
    return amount.toFixed(2) + ' DT';
  }
}
