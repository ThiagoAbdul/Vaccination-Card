import { Component, inject } from '@angular/core';
import { PersonService } from '../services/person.service';
import { SharedModule } from '../../../shared/shared.module';
import { Page } from '../../../shared/models/page';
import { Person } from '../models/person';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from '../../../shared/services/loader.service';
import { GenderPipe } from '../pipes/gender.pipe';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [SharedModule, GenderPipe],
  templateUrl: './people-list.component.html',
  styleUrl: './people-list.component.scss'
})
export class PeopleListComponent {
  private personService = inject(PersonService)
  protected showCreatePersonModal = false
  protected page$: Observable<Page<Person>>
  private loader = inject(LoaderService)

  constructor() {
    this.loader.displayLoading()
    this.page$ = this.personService.getPeoplePaginated(1, 10, "")
    .pipe(finalize(() => this.loader.hideLoading()))
  }

}
