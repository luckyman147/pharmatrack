import { Injectable } from '@angular/core';
import { Pharmacie } from '../models/Pharmacie';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PharmacieStateService {
  private selectedPharmacieSource = new BehaviorSubject<Pharmacie | null>(null);
  selectedPharmacie$ = this.selectedPharmacieSource.asObservable();

  setSelectedPharmacie(pharmacie: Pharmacie) {
    this.selectedPharmacieSource.next(pharmacie);
  }
  constructor() {}
}
