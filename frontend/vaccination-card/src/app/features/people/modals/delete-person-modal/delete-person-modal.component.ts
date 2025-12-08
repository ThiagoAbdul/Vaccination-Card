import { Component, inject, input, output } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { Person } from '../../models/person';
import { PersonService } from '../../services/person.service';
import { LoaderService } from '../../../../shared/services/loader.service';
import { finalize } from 'rxjs';
import { ErrorService } from '../../../../shared/services/error.service';

@Component({
  selector: 'app-delete-person-modal',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './delete-person-modal.component.html',
  styleUrl: './delete-person-modal.component.scss'
})
export class DeletePersonModalComponent {
  person = input.required<Person>()
  private personService = inject(PersonService)
  cancel = output()
  deleted = output<Person>()
  private loader = inject(LoaderService)
  private errrorService = inject(ErrorService)

  deletePerson(){
    this.loader.displayLoading()
    this.personService.deletePerson(this.person().id)
    .pipe(finalize(() => this.loader.hideLoading()))
    .subscribe({
      next: () => this.deleted.emit(this.person()),
      error: error => {
        this.errrorService.displayError(error)
      }
    })
  } 
}
