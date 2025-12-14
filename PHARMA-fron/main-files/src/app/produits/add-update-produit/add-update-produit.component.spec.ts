import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateProduitComponent } from './add-update-produit.component';

describe('AddUpdateProduitComponent', () => {
  let component: AddUpdateProduitComponent;
  let fixture: ComponentFixture<AddUpdateProduitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUpdateProduitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateProduitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
