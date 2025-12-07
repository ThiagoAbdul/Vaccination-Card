import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Page } from '../../../shared/models/page';
import { Person } from '../models/person';
import { CreatePersonRequest } from '../models/create-person-request';
import { CreatePersonResponse } from '../models/create-person-response';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  private http = inject(HttpClient)
  private apiBaseUrl = environment.vaccinationCardApiUrl + "/people"

  getPeoplePaginated(page: number, pageSize: number, name: string){
    const params = new HttpParams().append("page", page).append("pageSize", pageSize).append("name", name)
    return this.http.get<Page<Person>>(this.apiBaseUrl, { params })
  }

  createPerson(request: CreatePersonRequest){
    return this.http.post<CreatePersonResponse>(this.apiBaseUrl, request)
  }



}
