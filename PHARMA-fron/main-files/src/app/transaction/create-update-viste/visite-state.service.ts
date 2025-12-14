import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Visite } from '../visite/models/Visite';

@Injectable({
  providedIn: 'root',
})
export class VisiteStateService {
  private selectedVisiteSource = new BehaviorSubject<Visite | null>(null);
  selectedVisite$ = this.selectedVisiteSource.asObservable();

  setSelectedVisite(visite: Visite) {
    this.selectedVisiteSource.next(visite);
  }
  constructor() {}
}
