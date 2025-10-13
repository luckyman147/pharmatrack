import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateVisteComponent } from './create-update-viste.component';

describe('CreateUpdateVisteComponent', () => {
  let component: CreateUpdateVisteComponent;
  let fixture: ComponentFixture<CreateUpdateVisteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateVisteComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateVisteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
