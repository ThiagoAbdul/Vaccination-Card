import { Component, inject } from '@angular/core';
import { ToastService } from '../../../shared/services/toast.service';

export interface ToastMessage{
  title: string,
  description: string
}

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
  standalone: true
})
export class ToastComponent {
  messages: ToastMessage[] = [];
  private toastService = inject(ToastService)

  ngOnInit(){
    this.toastService.register(this)
  }

  show(message: ToastMessage) {
    this.messages.push(message);

    setTimeout(() => {
      this.messages = this.messages.filter(m => m !== message);
    }, 4000);
  }
}
