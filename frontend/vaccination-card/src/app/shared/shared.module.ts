import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../ui/components/input/input.component';
import { ButtonComponent } from '../ui/components/button/button.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    InputComponent,
    ButtonComponent
  ],
  exports: [
    CommonModule,
    InputComponent,
    ButtonComponent
  ]
})
export class SharedModule { }
