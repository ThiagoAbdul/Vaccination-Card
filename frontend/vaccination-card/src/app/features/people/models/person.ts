import { Gender } from "../enums/gender";

export interface Person {
  id: string,
  firstName: string,
  lastName: string,
  gender: Gender,
  birthDate: string
}
