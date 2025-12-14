import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommandeService } from '../../services/commande.service';
import { Commande, StatusCommande } from '../../models/commande.models';

@Component({
  selector: 'app-commande-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './commande-detail.component.html',
  styleUrl: './commande-detail.component.scss'
})
export class CommandeDetailComponent implements OnInit {
  commande: Commande | null = null;
  loading = false;
  error: string | null = null;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private commandeService: CommandeService
  ) {}
  
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadCommande(id);
    } else {
      this.error = 'ID de commande invalide';
    }
  }
  
  loadCommande(id: string): void {
    this.loading = true;
    this.error = null;
    
    this.commandeService.getCommandeById(id).subscribe({
      next: (commande) => {
        this.commande = commande;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erreur lors du chargement de la commande';
        this.loading = false;
        console.error('Error loading commande:', err);
      }
    });
  }
  
  editCommande(): void {
    if (this.commande) {
      this.router.navigate(['/Home/commandes', this.commande.id, 'edit']);
    }
  }
  
  deleteCommande(): void {
    if (!this.commande) return;
    
    if (confirm(`Êtes-vous sûr de vouloir supprimer la commande #${this.commande.codeComm}?`)) {
      this.commandeService.deleteCommande(this.commande.id).subscribe({
        next: () => {
          this.router.navigate(['/Home/commandes']);
        },
        error: (err) => {
          this.error = 'Erreur lors de la suppression de la commande';
          console.error('Error deleting commande:', err);
        }
      });
    }
  }
  
  goBack(): void {
    this.router.navigate(['/Home/commandes']);
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
  
  calculateLineTotal(unitPrice: number, allocations: any[]): number {
    const totalQty = allocations.reduce((sum, a) => sum + a.quantityAllocated, 0);
    return unitPrice * totalQty;
  }
  
  calculateLineTotalQty(allocations: any[]): number {
    return allocations.reduce((sum, a) => sum + a.quantityAllocated, 0);
  }
}
