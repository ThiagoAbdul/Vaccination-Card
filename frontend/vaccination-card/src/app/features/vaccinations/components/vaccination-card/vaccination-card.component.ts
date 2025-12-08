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

@Component({
  selector: 'app-vaccination-card',
  standalone: true,
  imports: [SharedModule, FormsModule, ReactiveFormsModule, VaccinationDosePipe ],
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

    form = new FormGroup({
    name: new FormControl(""),
    gender: new FormControl(""),
    age: new FormControl(""),
  })

  constructor(){
    this.route.params.subscribe(params => {
      const personId = params["personId"]
      this.loaderService.displayLoading()
      this.vaccinationService.getVaccinationCard(personId)
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
    })
  }


}
