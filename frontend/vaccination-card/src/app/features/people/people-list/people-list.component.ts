import { Component, inject } from '@angular/core';
import { PersonService } from '../services/person.service';
import { SharedModule } from '../../../shared/shared.module';
import { Page } from '../../../shared/models/page';
import { Person } from '../models/person';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from '../../../shared/services/loader.service';
import { GenderPipe } from '../pipes/gender.pipe';
import { CreatePersonModalComponent } from '../modals/create-person-modal/create-person-modal.component';
import { CreatePersonResponse } from '../models/create-person-response';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [SharedModule, GenderPipe, CreatePersonModalComponent],
  templateUrl: './people-list.component.html',
  styleUrl: './people-list.component.scss'
})
export class PeopleListComponent {
  private personService = inject(PersonService)
  protected showCreatePersonModal = false
  protected page$!: Observable<Page<Person>>
  private loader = inject(LoaderService)

  constructor() {
    this.loadPeople()
  }

  loadPeople(){
        this.loader.displayLoading()
    this.page$ = this.personService.getPeoplePaginated(1, 10, "")
    .pipe(finalize(() => this.loader.hideLoading()))
  }

  onCreate(person: CreatePersonResponse){
    this.showCreatePersonModal = false;
    this.loadPeople()
    alert(`Vacine ${person.name.fullName} cadastrada!`)
  }

}
