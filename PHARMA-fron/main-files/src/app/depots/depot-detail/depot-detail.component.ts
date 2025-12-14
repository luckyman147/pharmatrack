import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DepotService } from '../services/depot.service';
import { Depot } from '../models/depot';
import { Location } from '@angular/common';

@Component({
  selector: 'app-depot-detail',
  templateUrl: './depot-detail.component.html',
  styleUrls: ['./depot-detail.component.scss']
})
export class DepotDetailComponent implements OnInit {
  depot: Depot | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private depotService: DepotService,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.getDepot();
  }

  getDepot(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.depotService.getDepot(id).subscribe({
        next: (response) => {
          this.depot = response.depot;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error loading depot', err);
          this.error = 'Impossible de charger les détails du dépôt.';
          this.isLoading = false;
        }
      });
    } else {
      this.error = 'ID du dépôt non trouvé.';
      this.isLoading = false;
    }
  }

  goBack(): void {
    this.location.back();
  }

  onEdit(): void {
    if (this.depot) {
      this.router.navigate(['/depot/edit', this.depot.id]);
    }
  }

  get productCount(): number {
    return this.depot?.produitStock?.length || 0;
  }

  get totalStock(): number {
    return this.depot?.produitStock?.reduce((sum: number, item: any) => sum + item.quantity, 0) || 0;
  }

  get lowStockCount(): number {
    return this.depot?.produitStock?.filter((item: any) => item.quantity === 0).length || 0;
  }
}
