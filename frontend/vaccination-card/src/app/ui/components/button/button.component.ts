import { NgClass, NgStyle } from '@angular/common';
import { booleanAttribute, Component, input } from '@angular/core';

type ButtonType = "default" | "outline" | "discreet" | "discreet-alt" | "negative"
type ButtonSize = "fit" | "fill" | "default" | "large"

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [NgClass, NgStyle],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  text = input.required<string>()
  disabled = input<boolean>(false)
  type = input("default", { transform: (value: ButtonType | undefined) => value || "default"  })
  submit = input(false, { transform: booleanAttribute })
  width = input("100%")

  img = input<string>()
}
