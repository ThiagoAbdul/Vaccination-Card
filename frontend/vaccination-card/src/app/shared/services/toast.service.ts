import { Injectable } from '@angular/core';
import { ToastComponent, ToastMessage } from '../../ui/components/toast/toast.component';

@Injectable({ providedIn: 'root' })
export class ToastService {
  private toast!: ToastComponent;

  register(toast: ToastComponent) {
    this.toast = toast;
  }

  show(message: ToastMessage) {
    this.toast?.show(message);
  }
}
