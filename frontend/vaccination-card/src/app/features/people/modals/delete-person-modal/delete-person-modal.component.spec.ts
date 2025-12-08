import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeletePersonModalComponent } from './delete-person-modal.component';

describe('DeletePersonModalComponent', () => {
  let component: DeletePersonModalComponent;
  let fixture: ComponentFixture<DeletePersonModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeletePersonModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeletePersonModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
