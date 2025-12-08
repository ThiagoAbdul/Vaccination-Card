import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateVaccinationModalComponent } from './create-vaccination-modal.component';

describe('CreateVaccinationModalComponent', () => {
  let component: CreateVaccinationModalComponent;
  let fixture: ComponentFixture<CreateVaccinationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateVaccinationModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateVaccinationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
