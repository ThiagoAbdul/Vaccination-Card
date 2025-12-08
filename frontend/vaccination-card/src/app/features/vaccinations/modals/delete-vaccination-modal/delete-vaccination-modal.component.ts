import { Component, inject, input, output } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { LoaderService } from '../../../../shared/services/loader.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { VaccinationService } from '../../services/vaccination.service';
import { finalize } from 'rxjs';
import { VaccinationDetails } from '../../models/vaccination-card';

@Component({
  selector: 'app-delete-vaccination-modal',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './delete-vaccination-modal.component.html',
  styleUrl: './delete-vaccination-modal.component.scss'
})
export class DeleteVaccinationModalComponent {

  vaccinationId = input.required<string>()

  private vaccinationService = inject(VaccinationService)
  cancel = output()
  deleted = output()
  private loader = inject(LoaderService)
  private errrorService = inject(ErrorService)

  deleteVaccination() {
    this.loader.displayLoading()
    this.vaccinationService.deleteVaccination(this.vaccinationId())
      .pipe(finalize(() => this.loader.hideLoading()))
      .subscribe({
        next: () => this.deleted.emit(),
        error: error => {
          this.errrorService.displayError(error)
        }
      })
  }
}
