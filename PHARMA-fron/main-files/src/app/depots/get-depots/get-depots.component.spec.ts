import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetDepotsComponent } from './get-depots.component';

describe('GetDepotsComponent', () => {
  let component: GetDepotsComponent;
  let fixture: ComponentFixture<GetDepotsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetDepotsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GetDepotsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
