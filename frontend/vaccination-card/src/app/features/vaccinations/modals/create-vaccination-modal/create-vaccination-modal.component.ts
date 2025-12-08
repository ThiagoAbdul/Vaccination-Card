import { Component, Input, inject, input, output } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { LoaderService } from '../../../../shared/services/loader.service';
import { finalize } from 'rxjs';
import { VaccinationService } from '../../services/vaccination.service';
import { CreateVaccinationRequest } from '../../models/create-vaccination-request';
import { CreateVaccinationResponse } from '../../models/create-vaccination-response';
import { ErrorService } from '../../../../shared/services/error.service';

@Component({
  selector: 'app-create-vaccination-modal',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  templateUrl: './create-vaccination-modal.component.html',
  styleUrl: './create-vaccination-modal.component.scss'
})
export class CreateVaccinationModalComponent {

  private vaccinationService = inject(VaccinationService);
  private loader = inject(LoaderService);
  private errorService = inject(ErrorService)

  personId = input.required<string>()
  vaccineId = input.required<string>()

  cancel = output();
  created = output<CreateVaccinationResponse>();

  protected form = new FormGroup({
    vaccinationDate: new FormControl<string>("", {
      nonNullable: true,
      validators: [Validators.required]
    }),

    doseType: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required]
    }),

    doseNumber: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required]
    })
  });

  createVaccination() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: CreateVaccinationRequest = {
      personId: this.personId(),
      vaccineId: this.vaccineId(),
      vaccinationDate: this.form.controls.vaccinationDate.value,
      doseType: this.form.controls.doseType.value,
      doseNumber: this.form.controls.doseNumber.value
    };

    this.loader.displayLoading();

    this.vaccinationService.createVaccination(request)
      .pipe(finalize(() => this.loader.hideLoading()))
      .subscribe({
        next: (response) => this.created.emit(response),
        error: error => {
          this.errorService.displayError(error)
        }
      });
  }
}
