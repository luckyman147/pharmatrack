import { TestBed } from '@angular/core/testing';

import { VisiteStateService } from './visite-state.service';

describe('VisiteStateService', () => {
  let service: VisiteStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VisiteStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
