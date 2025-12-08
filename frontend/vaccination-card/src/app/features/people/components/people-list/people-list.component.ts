import { Component, inject } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { SharedModule } from '../../../../shared/shared.module';
import { Page } from '../../../../shared/models/page';
import { Person } from '../../models/person';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from '../../../../shared/services/loader.service';
import { GenderPipe } from '../../pipes/gender.pipe';
import { CreatePersonModalComponent } from '../../modals/create-person-modal/create-person-modal.component';
import { CreatePersonResponse } from '../../models/create-person-response';
import { Router } from '@angular/router';
import { DeletePersonModalComponent } from '../../modals/delete-person-modal/delete-person-modal.component';

@Component({
  selector: 'app-people-list',
  standalone: true,
  imports: [SharedModule, GenderPipe, CreatePersonModalComponent, DeletePersonModalComponent],
  templateUrl: './people-list.component.html',
  styleUrl: './people-list.component.scss'
})
export class PeopleListComponent {
  private personService = inject(PersonService)
  protected showCreatePersonModal = false
  protected page$!: Observable<Page<Person>>
  private loader = inject(LoaderService)
  private router = inject(Router)
  protected showDeletePersonModal = false
  protected currentPerson?: Person

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
    alert(`${person.name.fullName} cadastrado(a)!`)
  }

  goToVaccinationCard(personId: string){
    this.router.navigate(["vacinacao", "cartao", personId])
  }

  onClickDeletePerson(person: Person){
    this.currentPerson = person
    this.showDeletePersonModal = true
  }


  onDelete(person: Person){
    this.showDeletePersonModal = false;
    this.loadPeople()
    alert(`${person.name.fullName} Excluído(a)!`)
  }
}
