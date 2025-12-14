import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ZONES } from 'src/app/transaction/pharmacies/data/zoneData';

@Component({
  selector: 'app-filter-depots',
  templateUrl: './filter-depots.component.html',
  styleUrls: ['./filter-depots.component.scss'],
})
export class FilterDepotsComponent implements OnInit {
  @Output() filterApplied = new EventEmitter<any>();
  zones = Object.keys(ZONES);
  gouvernerats: string[] = [];
  delegations: string[] = [];
  form!: FormGroup;
  constructor(private fb: FormBuilder) {}
  applyFilter(): void {
    console.log('Filter values:', this.form.value);
    this.filterApplied.emit(this.form.value);
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      byName: ['', []],
      telephone: ['', Validators.pattern('^[0-9]+$')],

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
}
