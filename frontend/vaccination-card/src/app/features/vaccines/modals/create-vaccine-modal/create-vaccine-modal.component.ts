import { Component, inject, output } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { VaccineService } from '../../services/vaccine.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateVaccineRequest } from '../../models/create-vaccine-request';
import { LoaderService } from '../../../../shared/services/loader.service';
import { finalize } from 'rxjs';
import { CreateVaccineResponse } from '../../models/create-vaccine-response';

@Component({
  selector: 'app-create-vaccine-modal',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  templateUrl: './create-vaccine-modal.component.html',
  styleUrl: './create-vaccine-modal.component.scss'
})
export class CreateVaccineModalComponent {

  private vaccineService = inject(VaccineService)

  private loader = inject(LoaderService)

  cancel = output()
  created = output<CreateVaccineResponse>()

  protected form = new FormGroup({
    name: new FormControl<string>("", {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)]
    }),

    doses: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)]
    }),

    boosterDoses: new FormControl<number>(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)]
    })
  })

  createVaccine(){
    if(this.form.invalid){
      this.form.markAllAsTouched()
      return
    }

    const request: CreateVaccineRequest = {
      name: this.form.controls.name.value,
      doses: this.form.controls.doses.value,
      boosterDoses: this.form.controls.boosterDoses.value,
    }

    this.loader.displayLoading()

    this.vaccineService.createVaccine(request)
    .pipe(finalize(() => this.loader.hideLoading()))
    .subscribe({
      next: response => {
        this.created.emit(response)
      }
    })

  }
}

