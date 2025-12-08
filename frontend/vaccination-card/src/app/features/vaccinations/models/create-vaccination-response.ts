import { VaccinationDoseType } from "../enums/vaccination-dose-type";

export interface CreateVaccinationResponse {
  id: string;
  personId: string,
  vaccineId: string,
  vaccinationDate: string | Date,
  doseType: VaccinationDoseType,
  doseNumber: number
}
