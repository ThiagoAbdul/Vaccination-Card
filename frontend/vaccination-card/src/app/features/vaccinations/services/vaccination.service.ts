import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { VaccinationCard } from '../models/vaccination-card';

@Injectable({
  providedIn: 'root'
})
export class VaccinationService {

  private http = inject(HttpClient)
  private apiBaseUrl = environment.vaccinationCardApiUrl + "/vaccinations"

  getVaccinationCard(personId: string){
    const endpoint = `${this.apiBaseUrl}/people/${personId}/vaccination-card`
    return this.http.get<VaccinationCard>(endpoint)
  }
}
