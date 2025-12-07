import { booleanAttribute, Component, input, output } from '@angular/core';
import { ButtonComponent } from '../button/button.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [ButtonComponent, CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent {

  title = input.required<string>()
  primaryText = input<string>()
  secondaryText = input<string>()
  primaryAction = output()
  secondaryAction = output()
  width = input<string>("45rem")
  height = input<string>()
  close = output()
  primaryDisabled = input<boolean>(false)
  submit = input(false, { transform: booleanAttribute })
}
