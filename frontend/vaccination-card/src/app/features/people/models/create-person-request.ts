import { Gender } from "../enums/gender"

export interface CreatePersonRequest {
  name: {
    firstName: string,
    lastName: string
  },
  cpf: string,
  rg: string,
  gender: Gender,
  birthDate: string | Date
}