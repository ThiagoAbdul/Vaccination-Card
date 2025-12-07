import { Component, inject, output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from '../../../../shared/shared.module';
import { LoaderService } from '../../../../shared/services/loader.service';
import { CreatePersonResponse } from '../../models/create-person-response';
import { cpfValidator } from '../../validators/cpf.validator';
import { rgValidator } from '../../validators/rg.validator';
import { PersonService } from '../../services/person.service';
import { CreatePersonRequest } from '../../models/create-person-request';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-create-person-modal',
  standalone: true,
  imports: [ReactiveFormsModule, SharedModule],
  templateUrl: './create-person-modal.component.html',
  styleUrl: './create-person-modal.component.scss'
})
export class CreatePersonModalComponent {


  private loader = inject(LoaderService)
  private personService = inject(PersonService)

  form = new FormGroup({
    name: new FormGroup({
      firstName: new FormControl('', { validators: Validators.required, nonNullable: true }),
      lastName: new FormControl('', { validators: Validators.required, nonNullable: true })
    }),
    cpf: new FormControl('', {
      validators: [Validators.required, cpfValidator],
      nonNullable: true
    }),
    rg: new FormControl('', {
      validators: [rgValidator],
      nonNullable: true
    }),
    gender: new FormControl(0, { validators: Validators.required, nonNullable: true }), // 0, 1 ou 2
    birthDate: new FormControl('', { validators: Validators.required, nonNullable: true }) // yyyy-MM-dd
  });




  cancel = output()
  created = output<CreatePersonResponse>()

  createPerson() {
    if (this.form.invalid) {
      this.form.markAllAsTouched()
      return
    }

    const request: CreatePersonRequest = this.form.getRawValue()
    this.loader.displayLoading()
    this.personService.createPerson(request)
    .pipe(finalize(() => this.loader.hideLoading()))
    .subscribe({
      next: response => {
        this.created.emit(response)
      }
    })
  }
}
