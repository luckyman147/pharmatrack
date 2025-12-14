import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateDepotComponent } from './add-update-depot.component';

describe('AddUpdateDepotComponent', () => {
  let component: AddUpdateDepotComponent;
  let fixture: ComponentFixture<AddUpdateDepotComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUpdateDepotComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateDepotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
