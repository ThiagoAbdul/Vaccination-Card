import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../ui/components/input/input.component';
import { ButtonComponent } from '../ui/components/button/button.component';
import { CardComponent } from '../ui/components/card/card.component';
import { ModalComponent } from '../ui/components/modal/modal.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    InputComponent,
    ButtonComponent,
    CardComponent,
    ModalComponent
  ],
  exports: [
    CommonModule,
    InputComponent,
    ButtonComponent,
    CardComponent,
    ModalComponent
  ]
})
export class SharedModule { }
