import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Vaccine } from '../models/vaccine';

@Injectable({
  providedIn: 'root'
})
export class VaccineService {

  private http = inject(HttpClient)
  private apiBaseUrl = environment.vaccinationCardApiUrl + "/vaccines"

  listVaccines(){
    return this.http.get<Vaccine[]>(this.apiBaseUrl)
  }
}
