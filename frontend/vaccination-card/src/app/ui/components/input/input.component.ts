import { CommonModule } from '@angular/common';
import { booleanAttribute, Component, ElementRef, inject, input, output } from '@angular/core';
import { ControlValueAccessor, FormsModule, NgControl } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss'
})
export class InputComponent implements ControlValueAccessor {


  protected value: string = ""
  label = input<string>()
  placeholder = input("", { transform: (value: string | undefined) => value || "" })
  readOnly = input(false, { transform: booleanAttribute })
  type = input<string>()
  private ngControl = inject(NgControl, { optional: true })

  errorMessage = input<string>()

  get controlErrorMessage(): string | undefined {
    const errors = this.ngControl?.errors
    if (errors) {
      if (errors["customMessage"])
        return errors["customMessage"]
      if (errors["required"])
        return "Campo obrigatório"
    }

    return undefined
  }

  protected hasError = false

  touched = false

  blur = output<InputComponent>()



  constructor(protected elRef: ElementRef) {
    if (this.ngControl)
      this.ngControl.valueAccessor = this


  }

  onBlur() {
    this.touched = true
    this.blur.emit(this)
  }


  onValueChange(value: string) {
    this.onChange(value);
    this.onTouched();
  }



  // ControlValueAccessor methods
  writeValue(value: string): void {
    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  protected onChange(value: string) { }
  protected onTouched() { };
  protected isDisabled: boolean = false;

}
