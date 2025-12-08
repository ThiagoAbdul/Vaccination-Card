import { Gender } from "../../people/enums/gender";
import { VaccinationDoseType } from "../enums/vaccination-dose-type";

export interface VaccinationCard {
  vaccines: VaccineDetails[];
  person: PersonDetails
}

export interface VaccineDetails {
  id: string;
  name: string;
  doses: DoseDetails[];
}

export interface DoseDetails {
  available: boolean;
  type: VaccinationDoseType;
  doseNumber: number;
  vaccination: VaccinationDetails
}

export interface VaccinationDetails {
  id?: string;
  vaccinationDate?: string;
  applied: boolean;
}

export interface PersonDetails {
  name: {
    firstName: string,
    lastName: string,
    fullName: string
  },
  gender: Gender,
  birthDate: string
}