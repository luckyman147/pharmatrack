import { TestBed } from '@angular/core/testing';

import { PharmacieStateService } from './create-pharmacie/pharmacie-state.service';

describe('PharmacieStateService', () => {
  let service: PharmacieStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PharmacieStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
