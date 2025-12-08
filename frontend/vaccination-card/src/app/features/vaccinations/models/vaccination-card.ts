import { Gender } from "../../people/enums/gender";
import { VaccinationDoseType } from "../enums/vaccination-dose-type";

export interface VaccinationCard {
  vaccines: VaccineDetails[];
  person: PersonDetails,
  doses: DoseDetails[]
}

export interface VaccineDetails {
  id: string;
  name: string;
}

export interface DoseDetails {
  type: VaccinationDoseType;
  doseNumber: number;
  vaccinations: VaccinationDetails[]
}

export interface VaccinationDetails {
  id?: string;
  vaccinationDate?: string;
  applied: boolean;
  available: boolean;
  absent: boolean,
  vaccineId: string
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