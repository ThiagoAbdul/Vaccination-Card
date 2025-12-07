import { AbstractControl, ValidationErrors, ValidatorFn, FormGroup } from '@angular/forms';


export function passwordMatchValidator(): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null; // deixa outros validators tratarem
    }

    if (password !== confirmPassword) {
      return { customMessage: "As senhas não coincidem" };
    }

    return null;
  };
}
