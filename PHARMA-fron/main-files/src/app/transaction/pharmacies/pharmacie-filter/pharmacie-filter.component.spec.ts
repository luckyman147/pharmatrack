import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PharmacieFilterComponent } from './pharmacie-filter.component';

describe('PharmacieFilterComponent', () => {
  let component: PharmacieFilterComponent;
  let fixture: ComponentFixture<PharmacieFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PharmacieFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PharmacieFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
