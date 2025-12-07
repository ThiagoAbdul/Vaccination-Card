import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateVaccineModalComponent } from './create-vaccine-modal.component';

describe('CreateVaccineModalComponent', () => {
  let component: CreateVaccineModalComponent;
  let fixture: ComponentFixture<CreateVaccineModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateVaccineModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateVaccineModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
