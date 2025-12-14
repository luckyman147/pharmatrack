import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterDepotsComponent } from './filter-depots.component';

describe('FilterDepotsComponent', () => {
  let component: FilterDepotsComponent;
  let fixture: ComponentFixture<FilterDepotsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilterDepotsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterDepotsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
