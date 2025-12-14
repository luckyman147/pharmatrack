import { TestBed } from '@angular/core/testing';

import { AddUpdateProduitService } from './add-update-produit.service';

describe('AddUpdateProduitService', () => {
  let service: AddUpdateProduitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddUpdateProduitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
