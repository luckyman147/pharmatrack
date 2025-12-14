import { Component, OnInit } from '@angular/core';
import { Pharmacie } from '../pharmacies/models/Pharmacie';
import Swal from 'sweetalert2';
import { User } from 'src/app/shared/Models/User';
import { CreateVisiteDto } from '../visite/DTO/createVisiteDto';
import { VisiteService } from '../visite/visite.service';
import jsPDF from 'jspdf';
import { ActivatedRoute } from '@angular/router';
import { VisiteStateService } from './visite-state.service';
import { Visite } from '../visite/models/Visite';
import { UpdateVisteDto } from '../visite/DTO/updateViisteDto';
import { DelegueInfo } from '../visite/models/DelegueInfo';
import { toDelegueInfo } from 'src/app/shared/extensions/user.Extensions';

@Component({
  selector: 'app-create-update-viste',
  templateUrl: './create-update-viste.component.html',
  styleUrls: ['./create-update-viste.component.scss'],
})
export class CreateUpdateVisteComponent implements OnInit {
  selectedPharmacie?: Pharmacie;
  delegue?: DelegueInfo;
  description: string = '';
  visiteId?: string;
  visiteData?: Visite;
  status?: string;
  ActionType: string = 'Creér';
  onDateChange(selectedDate: string) {
    this.today = selectedDate;
    // handle selected date
  }
  today: string = '';
  ngOnInit(): void {
    this.visiteId = this.route.snapshot.paramMap.get('id') || undefined;
    if (this.visiteId) {
      // Update mode
      // Either fetch from API or from state service
      this.visiteState.selectedVisite$.subscribe((visite) => {
        if (visite && visite.id === this.visiteId) {
          this.populateVisite(visite);
        } else {
          // optionally fetch from API
          // this.visiteService.getVisiteById(this.visiteId).subscribe(...)
        }
      });
    } else {
      const userData = localStorage.getItem('user');
      if (userData) {
        this.delegue = toDelegueInfo(JSON.parse(userData) as User);
      }
      this.today = new Date().toISOString().split('T')[0];
      // this.delegue.profile.
    }
  }
  populateVisite(visite: Visite) {
    this.visiteData = visite;
    this.selectedPharmacie = visite.pharamcieBasicInfo as any;
    this.description = visite.description;
    this.today = new Date(visite.visitDate).toISOString().split('T')[0];
    this.ActionType = 'Mis a jour';

    this.delegue = visite.delegueteInfo as DelegueInfo;
  }

  saveVisite() {
    if (this.visiteId) {
      this.updateVisite();
    } else {
      this.createVisite();
    }
  }
  updateVisite() {
    if (!this.visiteId) {
      Swal.fire('Erreur', 'Le Id must be specified', 'error');
      return;
    }
    if (!this.selectedPharmacie) {
      Swal.fire('Erreur', 'Veuillez sélectionner une pharmacie.', 'error');
      return;
    }
    if (!this.today) {
      Swal.fire('Erreur', 'Veuillez sélectionner une date de visite.', 'error');
      return;
    }
    if (!this.delegue || !this.delegue.id) {
      Swal.fire('Erreur', 'Informations du délégué non disponibles.', 'error');
      return;
    }
    if (!this.description || this.description.trim().length === 0) {
      Swal.fire(
        'Erreur',
        'Veuillez entrer une description pour la visite.',
        'error'
      );
      return;
    }
    const visiteData: UpdateVisteDto = {
      description: this.description,
      id: this.visiteId,
      date: this.today,
      reportUrl: null,
    };
    Swal.fire({
      title: 'Mise a jour en cours...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
        this.visiteService.updateVisite(visiteData).subscribe({
          next: (response) => {
            Swal.fire('Succès', 'Visite mis a jour avec succès.', 'success');
            this.goBack();
          },
          error: (error) => {
            console.error('Error creating visite:', error);
            Swal.fire(
              'Erreur',
              'Une erreur est survenue lors de la mise a jour de la visite.',
              'error'
            );
          },
        });
      },
    });
  }

  createVisite() {
    if (!this.selectedPharmacie) {
      Swal.fire('Erreur', 'Veuillez sélectionner une pharmacie.', 'error');
      return;
    }
    if (!this.today) {
      Swal.fire('Erreur', 'Veuillez sélectionner une date de visite.', 'error');
      return;
    }
    if (!this.delegue || !this.delegue.id) {
      Swal.fire('Erreur', 'Informations du délégué non disponibles.', 'error');
      return;
    }
    if (!this.description || this.description.trim().length === 0) {
      Swal.fire(
        'Erreur',
        'Veuillez entrer une description pour la visite.',
        'error'
      );
      return;
    }
    const visiteData: CreateVisiteDto = {
      description: this.description || '',
      visitDate: this.today,
      delegueteInfo: {
        id: this.delegue?.id,
        fullName: this.delegue?.fullName,
        avatarUrl: this.delegue?.avatarUrl,
      },
      pharamcieBasicInfo: {
        id: this.selectedPharmacie.id,

        name: this.selectedPharmacie.name,
      },
    };
    console.log('Creating visite with data:', visiteData);
    Swal.fire({
      title: 'Création en cours...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
        this.visiteService.createVisite(visiteData).subscribe({
          next: (response) => {
            console.log('Visite created successfully:', response);
            Swal.fire('Succès', 'Visite créée avec succès.', 'success');
            // Optionally reset form fields here
            this.selectedPharmacie = undefined;
            this.description = '';
            this.today = new Date().toISOString().split('T')[0];
          },
          error: (error) => {
            console.error('Error creating visite:', error);
            Swal.fire(
              'Erreur',
              'Une erreur est survenue lors de la création de la visite.',
              'error'
            );
          },
        });
      },
    });
  }
  onSelectUpdate() {
    console.log('sssss');
    this.status = 'Mis à jour';
  }
  onCreatedPharmacie() {
    this.returnPage();
  }
  goBack() {
    window.history.back();
  }
  onPharmacieSelected(p: Pharmacie) {
    this.selectedPharmacie = p;
  }
  returnPage() {
    this.status = null;
  }
  onStatusChanged() {
    this.status = 'Create';
  }
  downloadPDF() {
    console.log('eeeeee');
    const doc = new jsPDF({
      orientation: 'portrait',
      unit: 'mm',
      format: 'a4',
    });

    const marginLeft = 20;
    const pageWidth = doc.internal.pageSize.getWidth();
    let y = 25;

    // ---- ✅ Add Organization Logo ----
    const logoUrl = 'assets/images//logo.png'; // <-- replace with your actual logo path
    const logoWidth = 50;
    const logoHeight = 50;

    const img = new Image();
    img.src = logoUrl;

    img.onload = () => {
      // Draw the logo at the top right corner
      doc.addImage(
        img,
        'PNG',
        pageWidth - logoWidth - 20,
        5,
        logoWidth,
        logoHeight
      );

      // ---- Title ----
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(18);
      doc.text('Fiche de Visite Pharmacie', marginLeft, 25);

      // Optional separator line under title

      // ---- Content ----
      y = 45;
      doc.setFont('helvetica', 'normal');
      doc.setFontSize(12);

      doc.text(`Délégué : ${this.delegue?.fullName || '-'}`, marginLeft, y);
      y += 10;

      doc.text(
        `Pharmacie : ${this.selectedPharmacie?.name || '-'}`,
        marginLeft,
        y
      );
      y += 10;

      const adresse = `${this.selectedPharmacie?.location?.rue || '-'}, ${
        this.selectedPharmacie?.location?.delegation || '-'
      }, ${this.selectedPharmacie?.location?.gouvernerat || '-'}, ${
        this.selectedPharmacie?.location?.zone || '-'
      }`;
      doc.text('Adresse :', marginLeft, y);
      y += 8;
      const splitAdresse = doc.splitTextToSize(adresse, 170);
      doc.text(splitAdresse, marginLeft, y);
      y += splitAdresse.length * 7;

      doc.text(`Date de visite : ${this.today}`, marginLeft, y);
      y += 15;

      doc.setFont('helvetica', 'bold');
      doc.text('Description :', marginLeft, y);
      y += 8;

      doc.setFont('helvetica', 'normal');
      const splitDesc = doc.splitTextToSize(this.description || '-', 170);
      doc.text(splitDesc, marginLeft, y);
      doc.setFont('helvetica', 'italic');
      doc.setFontSize(12);
      const pageHeight = doc.internal.pageSize.getHeight();
      const signatureY = pageHeight - 30; // place near bottom
      const signatureText = 'Signature du Délégué';
      const textWidth = doc.getTextWidth(signatureText);
      doc.text(signatureText, pageWidth - textWidth - marginLeft, signatureY);
      doc.line(
        pageWidth - textWidth - marginLeft,
        signatureY + 2,
        pageWidth - marginLeft,
        signatureY + 2
      );

      // ---- Save & Open ----
      doc.save(`Visite-${this.delegue?.fullName || 'SansNom'}.pdf`);
    };
  }

  constructor(
    private visiteService: VisiteService,
    private route: ActivatedRoute,
    private visiteState: VisiteStateService
  ) {}
}
