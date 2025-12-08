import { Pipe, PipeTransform } from '@angular/core';
import { DoseDetails } from '../models/vaccination-card';
import { VaccinationDoseType } from '../enums/vaccination-dose-type';

@Pipe({
  name: 'vaccinationDose',
  standalone: true
})
export class VaccinationDosePipe implements PipeTransform {

  transform(value: DoseDetails): string {
    return `${value.doseNumber}${value.type == VaccinationDoseType.Primary? 'ª' : 'º'} 
    ${value.type == VaccinationDoseType.Primary? 'Dose' : 'Reforço'}`
  }

}
