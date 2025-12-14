import { Component, Input, OnInit } from '@angular/core';
import { GroupedResult } from '../models/GroupedResult';
import { VisiteService } from '../visite.service';
import { Visite } from '../models/Visite';
import Swal from 'sweetalert2';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { VisiteStateService } from '../../create-update-viste/visite-state.service';

@Component({
  selector: 'app-components',
  templateUrl: './components.component.html',
  styleUrls: ['./components.component.scss'],
})
export class ComponentsComponent implements OnInit {
  @Input() visites: GroupedResult[];
  constructor(
    private visiteService: VisiteService,
    private router: Router,
    private visiteState: VisiteStateService
  ) {}
  deleteVisite(visite: Visite): void {
    Swal.fire({
      title: 'Supprimer la visite ?',
      text: 'Cette action est irréversible.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Oui, supprimer',
      cancelButtonText: 'Annuler',
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
    }).then((result) => {
      if (result.isConfirmed) {
        this.visiteService.deleteVisite(visite.id).subscribe(() => {
          Swal.fire(
            'Supprimée !',
            'La visite a été supprimée avec succès.',
            'success'
          );
          this.visites = this.visites.filter((group) =>
            group.items.some((item) => item.id !== visite.id)
          );
        });
      }
    });
  }
  updateViste(visite: Visite) {
    // Modifier-Visite/:id
    this.visiteState.setSelectedVisite(visite);
    this.router.navigate(['Transaction/Modifier-Visite', visite.id]);
  }

  ngOnInit(): void {}
}
