import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ZONES } from '../data/zoneData';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Horaire, PharmacieType } from '../models/Pharmacie';

@Component({
  selector: 'app-pharmacie-filter',
  templateUrl: './pharmacie-filter.component.html',
  styleUrls: ['./pharmacie-filter.component.scss'],
})
export class PharmacieFilterComponent implements OnInit {
  @Output() filterApplied = new EventEmitter<any>();

  form!: FormGroup;
  types = Object.values(PharmacieType);
  horaires = Object.values(Horaire);

  zones = Object.keys(ZONES);
  gouvernerats: string[] = [];
  delegations: string[] = [];

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      byName: [''],
      type: [''],
      horaire: [''],
      zone: [''],
    
      gouvernerat: [''],
      delegation: [''],
    });

    this.form.get('zone')?.valueChanges.subscribe((zone) => {
      this.gouvernerats = zone ? Object.keys(ZONES[zone]) : [];
      this.form.get('gouvernerat')?.setValue('');
      this.delegations = [];
      this.form.get('delegation')?.setValue('');
    });

    this.form.get('gouvernerat')?.valueChanges.subscribe((gouv) => {
      const zone = this.form.get('zone')?.value;
      this.delegations = zone && gouv ? ZONES[zone][gouv] : [];
      this.form.get('delegation')?.setValue('');
    });
  }

  submit(): void {
    if (this.form.valid) {
      this.filterApplied.emit(this.form.value);
    }
  }
}
