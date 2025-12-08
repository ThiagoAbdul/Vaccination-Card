import { VaccinationDoseType } from "../enums/vaccination-dose-type";

export interface CreateVaccinationRequest {
  personId: string,
  vaccineId: string,
  vaccinationDate: string | Date,
  doseType: VaccinationDoseType,
  doseNumber: number
}