import { AbstractControl, ValidationErrors } from "@angular/forms";

export function rgValidator(control: AbstractControl): ValidationErrors | null {
  const rg = control.value?.toString().trim();

  // Vou abstrair a validação do RG

  return null; // válido
}
