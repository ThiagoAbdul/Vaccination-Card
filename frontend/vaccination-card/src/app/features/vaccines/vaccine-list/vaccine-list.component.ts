import { Component, inject } from '@angular/core';
import { VaccineService } from '../services/vaccine.service';
import { delay, finalize, Observable } from 'rxjs';
import { Vaccine } from '../models/vaccine';
import { AsyncPipe } from '@angular/common';
import { LoaderService } from '../../../shared/services/loader.service';

@Component({
  selector: 'app-vaccine-list',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './vaccine-list.component.html',
  styleUrl: './vaccine-list.component.scss'
})
export class VaccineListComponent {
  private vaccineService = inject(VaccineService)
  protected vaccines$: Observable<Vaccine[]>
  private loader = inject(LoaderService)
  constructor(){
    this.loader.displayLoading()
    this.vaccines$ = this.vaccineService
    .listVaccines()
    .pipe(finalize(() => this.loader.hideLoading()))
  }
}
