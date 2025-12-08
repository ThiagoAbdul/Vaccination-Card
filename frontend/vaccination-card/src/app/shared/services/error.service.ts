import { inject, Injectable } from '@angular/core';
import { ResponseError } from '../models/response-error';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  private toasService = inject(ToastService)

  displayError(error: any){
    if(error.error?.message){
      this.toasService.show({
        title: error.error.message,
        description: error.error?.errors?.at(0)
      })
    }
  }
}
