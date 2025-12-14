import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePharmacieComponent } from './create-pharmacie.component';

describe('CreatePharmacieComponent', () => {
  let component: CreatePharmacieComponent;
  let fixture: ComponentFixture<CreatePharmacieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePharmacieComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreatePharmacieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
