import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { Horaire, Pharmacie, PharmacieType } from '../models/Pharmacie';
import { ZONES } from '../data/zoneData';
import { PharmacieService } from '../pharmacie.service';
import { CreatePharmacieDto } from '../dto/createPharmacieDto';
import { Address } from 'src/app/shared/Models/UserProfile';
import { PharmacieStateService } from './pharmacie-state.service';

@Component({
  selector: 'app-create-pharmacie',
  templateUrl: './create-pharmacie.component.html',
  styleUrls: ['./create-pharmacie.component.scss'],
})
export class CreatePharmacieComponent implements OnInit {
  @Output() PharmcieCreated = new EventEmitter<any>();
  @Output() PharmcieUpdated = new EventEmitter<any>();
  @Output() ChangeToUpdateStatus = new EventEmitter<any>();
  
  @Output() return = new EventEmitter<void>();
  form!: FormGroup;
  types = Object.values(PharmacieType);
  horaires = Object.values(Horaire);
  zones = Object.keys(ZONES);
  gouvernerats: string[] = [];
  delegations: string[] = [];
pharmacieUpdatedId?:string
  constructor(
    private fb: FormBuilder,
    private pharmacieStateService:PharmacieStateService,
    private pharmacieService: PharmacieService
  ) {}

  populatePharmacie(phar: Pharmacie) {
  if (!this.form) return;

  this.form.patchValue({
    byName: phar.name || '',
    ownerName: phar.ownerName || '',
    type: phar.pharmacieType || '',
    horaire: phar.horaire || '',
    zone: phar.location?.zone || '',
    gouvernerat: phar.location?.gouvernerat || '',
    delegation: phar.location?.delegation || '',
    rue: phar.location?.rue || '',
    phone: phar.telephone || '',
  });

  // Optionally populate dependent selects
  if (phar.location?.zone) {
    this.gouvernerats = Object.keys(ZONES[phar.location.zone]);
  }
  if (phar.location?.zone && phar.location?.gouvernerat) {
    this.delegations = ZONES[phar.location.zone][phar.location.gouvernerat];
  }
  }
  ngOnInit(): void {

  this.pharmacieUpdatedId = localStorage.getItem('PharmacieId');
  if (this.pharmacieUpdatedId) {
    this.pharmacieStateService.selectedPharmacie$.subscribe((phar) => {
      if (phar && phar.id == this.pharmacieUpdatedId) {
        this.populatePharmacie(phar);
        this.ChangeToUpdateStatus.emit();
      }
    });
  }
  else{

    this.form = this.fb.group({
      byName: ['', Validators.required],
      ownerName: ['', Validators.required],
      type: ['', Validators.required],
      horaire: ['', Validators.required],
      zone: ['', Validators.required],
      gouvernerat: ['', Validators.required],
      delegation: ['', Validators.required],
      rue: ['', Validators.required],
      phone: [''], // optional
    });
  }

    this.form.get('zone')?.valueChanges.subscribe((zone) => {
      this.gouvernerats = zone ? Object.keys(ZONES[zone]) : [];
      this.form.patchValue({ gouvernerat: '', delegation: '' });
      this.delegations = [];
    });

    this.form.get('gouvernerat')?.valueChanges.subscribe((gouv) => {
      const zone = this.form.get('zone')?.value;
      this.delegations = zone && gouv ? ZONES[zone][gouv] : [];
      this.form.patchValue({ delegation: '' });
    });
  }

  submit() {
    if (this.form.invalid) {
      Swal.fire({
        icon: 'error',
        title: 'Champs manquants',
        text: 'Veuillez remplir tous les champs obligatoires.',
        confirmButtonColor: '#d33',
      });
      return;
    }

    const pharmaciedto: CreatePharmacieDto = {
      name: this.form.value.byName,
      ownerName: this.form.value.ownerName,
      pharmacieType: this.form.value.type,
      horaire: this.form.value.horaire,
      location: {
        zone: this.form.value.zone,
        gouvernerat: this.form.value.gouvernerat,
        delegation: this.form.value.delegation,
        rue: this.form.value.rue,
      },
      telephone: this.form.value.phone || null, // optional
    };

    Swal.fire({
      title: 'Création en cours...',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    this.pharmacieService.createPharmacie(pharmaciedto).subscribe({
      next: (response) => {
        Swal.fire({
          icon: 'success',
          title: 'Succès',
          text: 'Pharmacie créée avec succès!',
          confirmButtonColor: '#3085d6',
        });
        this.PharmcieCreated.emit(response);
        this.form.reset();
      },
      error: (error) => {
        console.error('Erreur création pharmacie:', error);
        Swal.fire({
          icon: 'error',
          title: 'Erreur',
          text: 'Une erreur est survenue lors de la création de la pharmacie.',
          confirmButtonColor: '#d33',
        });
      },
    });
  }
}
