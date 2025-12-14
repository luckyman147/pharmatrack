import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateCategorieComponent } from './add-update-categorie.component';

describe('AddUpdateCategorieComponent', () => {
  let component: AddUpdateCategorieComponent;
  let fixture: ComponentFixture<AddUpdateCategorieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUpdateCategorieComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateCategorieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
