import { Component, inject } from '@angular/core';
import { VaccineService } from '../../services/vaccine.service';
import { delay, finalize, Observable } from 'rxjs';
import { Vaccine } from '../../models/vaccine';
import { AsyncPipe } from '@angular/common';
import { LoaderService } from '../../../../shared/services/loader.service';
import { ButtonComponent } from "../../../../ui/components/button/button.component";
import { CreateVaccineModalComponent } from '../../modals/create-vaccine-modal/create-vaccine-modal.component';
import { CreateVaccineResponse } from '../../models/create-vaccine-response';

@Component({
  selector: 'app-vaccine-list',
  standalone: true,
  imports: [AsyncPipe, ButtonComponent, CreateVaccineModalComponent],
  templateUrl: './vaccine-list.component.html',
  styleUrl: './vaccine-list.component.scss'
})
export class VaccineListComponent {
  private vaccineService = inject(VaccineService)
  protected vaccines$!: Observable<Vaccine[]>
  private loader = inject(LoaderService)
  protected showCreateVaccineModal = false
  constructor(){
    this.loadVaccines()
  }

  private loadVaccines(){
    this.loader.displayLoading()
    this.vaccines$ = this.vaccineService
    .listVaccines()
    .pipe(finalize(() => this.loader.hideLoading()))
  }

  onCreate(vaccine: CreateVaccineResponse){
    this.showCreateVaccineModal = false;
    this.loadVaccines()
    alert(`Vacine ${vaccine.name} cadastrada!`)

  }
}
