import { Gender } from "../enums/gender"

export interface CreatePersonResponse{
  id: string,
  name: {
    firstName: string,
    lastName: string,
    fullName: string
  },
  gender: Gender,
  birthDate: string

}