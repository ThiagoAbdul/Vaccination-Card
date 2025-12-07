import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  private _loading = signal(false)

  get loading(){
    return this._loading.asReadonly()
  }

  displayLoading(){
    this._loading.set(true)
  }

  hideLoading(){
    this._loading.set(false)

  }
}
