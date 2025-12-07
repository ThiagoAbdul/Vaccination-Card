import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from "@angular/forms"

export function passwordValidator(): ValidatorFn {

  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value as string;

    if (!value) {
      return null;
    }

    if (value.length < 8) {
      return { customMessage: "Senha deve ter no mínimo 8 caracteres" };
    }

    if (!/[A-Z]/.test(value)) {
      return { customMessage: "Senha deve conter pelo menos uma letra maiúscula" };
    }

    if (!/[a-z]/.test(value)) {
      return { customMessage: "Senha deve conter pelo menos uma letra minúscula" };
    }

    if (!/\d/.test(value)) {
      return { customMessage: "Senha deve conter pelo menos um número" };
    }

    if (!/[^A-Za-z0-9]/.test(value)) {
      return { customMessage: "Senha deve conter pelo menos um caractere especial" };
    }

    return null; // válido
  };
}
