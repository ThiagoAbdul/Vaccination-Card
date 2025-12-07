import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../ui/components/input/input.component';
import { ButtonComponent } from '../ui/components/button/button.component';
import { CardComponent } from '../ui/components/card/card.component';
import { ModalComponent } from '../ui/components/modal/modal.component';
import { BrazillianFormatDate } from './pipes/brazillian-format-date.pipe';



@NgModule({
  declarations: [
    BrazillianFormatDate
  ],
  imports: [
    CommonModule,
    InputComponent,
    ButtonComponent,
    CardComponent,
    ModalComponent,
  ],
  exports: [
    CommonModule,
    InputComponent,
    ButtonComponent,
    CardComponent,
    ModalComponent,
    BrazillianFormatDate
  ]
})
export class SharedModule { }
