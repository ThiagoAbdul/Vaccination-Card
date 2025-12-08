import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { VaccinationCard } from '../models/vaccination-card';
import { CreateVaccinationRequest } from '../models/create-vaccination-request';
import { CreateVaccinationResponse } from '../models/create-vaccination-response';

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

  createVaccination(request: CreateVaccinationRequest){
    return this.http.post<CreateVaccinationResponse>(this.apiBaseUrl, request)
  }
}
