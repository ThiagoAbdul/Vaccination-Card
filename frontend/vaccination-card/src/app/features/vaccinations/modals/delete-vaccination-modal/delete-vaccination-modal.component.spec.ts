import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteVaccinationModalComponent } from './delete-vaccination-modal.component';

describe('DeleteVaccinationModalComponent', () => {
  let component: DeleteVaccinationModalComponent;
  let fixture: ComponentFixture<DeleteVaccinationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteVaccinationModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeleteVaccinationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
