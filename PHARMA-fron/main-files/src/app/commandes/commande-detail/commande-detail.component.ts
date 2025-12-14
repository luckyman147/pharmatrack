import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommandeService } from '../commande.service';
import { Commande, StatusCommande } from '../models/commande.models';
import { Adress } from 'src/app/transaction/pharmacies/models/Pharmacie';

@Component({
  selector: 'app-commande-detail',
  templateUrl: './commande-detail.component.html',
  styleUrls: ['./commande-detail.component.scss']
})
export class CommandeDetailComponent implements OnInit {
  commande: Commande | null = null;
  loading = false;
  error: string | null = null;
  isAdmin = false; // Check if user is admin
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private commandeService: CommandeService
  ) {}
  
  ngOnInit(): void {
    // Check if user is admin
    const userStr = localStorage.getItem('user');
    if (userStr) {
      try {
        const user = JSON.parse(userStr);
        this.isAdmin = user.role === 'Admin';
      } catch (error) {
        console.error('Error parsing user from localStorage:', error);
        this.isAdmin = false;
      }
    }
    
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
      this.router.navigate(['/commandes', this.commande.id, 'edit']);
    }
  }
  
  deleteCommande(): void {
    if (!this.commande) return;
    
    if (confirm(`Êtes-vous sûr de vouloir supprimer la commande #${this.commande.codeComm}?`)) {
      this.commandeService.deleteCommande(this.commande.id).subscribe({
        next: () => {
          this.router.navigate(['/commandes']);
        },
        error: (err) => {
          this.error = 'Erreur lors de la suppression de la commande';
          console.error('Error deleting commande:', err);
        }
      });
    }
  }
  
  goBack(): void {
    this.router.navigate(['/commandes']);
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
      case StatusCommande.Annulee:
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
      case StatusCommande.Annulee:
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
  calculateDepotsLineTotal(unitPrice: number, quantityAllocated: number): number {
    return unitPrice * quantityAllocated;
  }

  
  calculateLineTotalQty(allocations: any[]): number {
    return allocations.reduce((sum, a) => sum + a.quantityAllocated, 0);
  }
  
  
  formatAddress(address: Adress): string {
    const parts = [
      address.zone,
      address.rue,
      address.delegation,
      address.gouvernerat
    ].filter(part => part && part.trim() !== '');
    
    return parts.length > 0 ? parts.join(', ') : 'Non spécifié';
  }

  // Expose Enum to template
  StatusCommande = StatusCommande;

  updateStatus(newStatus: StatusCommande): void {
    if (!this.commande) return;

    if (confirm(`Êtes-vous sûr de vouloir changer le statut de la commande #${this.commande.codeComm}?`)) {
        this.loading = true; // Show loading
        this.commandeService.updateStatus(this.commande.id, newStatus).subscribe({
            next: (success) => {
                this.loading = false;
                if (success && this.commande) {
                    this.commande.status = newStatus; // Update local state
                }
            },
            error: (err) => {
                this.loading = false;
                this.error = "Erreur lors de la mise à jour du statut";
                console.error(err);
            }
        });
    }
  }
}
