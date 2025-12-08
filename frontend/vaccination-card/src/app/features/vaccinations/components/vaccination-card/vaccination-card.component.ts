import { Component, inject } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VaccinationCard } from '../../models/vaccination-card';
import { ActivatedRoute } from '@angular/router';
import { VaccinationService } from '../../services/vaccination.service';
import { LoaderService } from '../../../../shared/services/loader.service';
import { finalize } from 'rxjs';
import { dateToAge, isoStringToDate } from '../../../../shared/utils/date-utils';
import { GenderPipe } from '../../../people/pipes/gender.pipe';
import { BrazillianFormatDate } from '../../../../shared/pipes/brazillian-format-date.pipe';
import { VaccinationDosePipe } from '../../pipes/vaccination-dose.pipe';
import { CreateVaccinationModalComponent } from '../../modals/create-vaccination-modal/create-vaccination-modal.component';
import { CreateVaccinationResponse } from '../../models/create-vaccination-response';
import { DeleteVaccinationModalComponent } from '../../modals/delete-vaccination-modal/delete-vaccination-modal.component';

@Component({
  selector: 'app-vaccination-card',
  standalone: true,
  imports: [SharedModule, FormsModule, ReactiveFormsModule, VaccinationDosePipe, CreateVaccinationModalComponent, DeleteVaccinationModalComponent ],
  templateUrl: './vaccination-card.component.html',
  styleUrl: './vaccination-card.component.scss',
  providers: [GenderPipe]
})
export class VaccinationCardComponent {
  
  protected vaccinationcard?: VaccinationCard
  private vaccinationService = inject(VaccinationService)
  private route = inject(ActivatedRoute)
  private loaderService = inject(LoaderService)
  private genderPipe = inject(GenderPipe)
  protected showCreateVaccinationModal = false
  protected showDeleteVaccinationModal = false
  protected currentVaccineId?: string
  protected currentVaccinationId?: string
  protected personId!: string
  
  form = new FormGroup({
    name: new FormControl(""),
    gender: new FormControl(""),
    age: new FormControl(""),
  })

  constructor(){
    this.route.params.subscribe(params => {
      this.personId = params["personId"]
      this.loadVaccinationCard()
      
    })
  }

  loadVaccinationCard(){
    this.loaderService.displayLoading()
      this.vaccinationService.getVaccinationCard(this.personId)
      .pipe(finalize(() => this.loaderService.hideLoading()))
      .subscribe({
        next: response => {
          this.vaccinationcard = response
          const person = response.person
          this.form.patchValue({
            name: person.name.fullName,
            age: dateToAge(isoStringToDate(person.birthDate)) + " anos",
            gender: this.genderPipe.transform(person.gender)
          })
        }
      })
  }

  onClickVaccineAbsent(vaccineId: string){
    this.showCreateVaccinationModal = true
    this.currentVaccineId = vaccineId
  }

  onCreate(response: CreateVaccinationResponse){
    this.showCreateVaccinationModal = false
    this.currentVaccineId = undefined
    alert("Vacinação cadastrada")
    this.loadVaccinationCard()
  
  }

  onClickInVaccination(vaccinationId: string){
    this.currentVaccinationId = vaccinationId
    this.showDeleteVaccinationModal = true
  }

  onDeleted(){
    this.showDeleteVaccinationModal = false
    this.currentVaccinationId = undefined
    alert("Vacinação excluída")
    this.loadVaccinationCard()
  }

}
