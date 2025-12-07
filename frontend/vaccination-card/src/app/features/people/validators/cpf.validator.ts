import { AbstractControl, ValidationErrors } from '@angular/forms';

export function cpfValidator(control: AbstractControl): ValidationErrors | null {
  const cpf = control.value?.replace(/\D/g, ''); // remove tudo que não é número
  if (!cpf || cpf.length !== 11) return { customMessage: "CPF inválido" };

  // Checa se todos os dígitos são iguais
  if (/^(\d)\1{10}$/.test(cpf)) return { customMessage: "CPF inválido" };

  let sum = 0;
  for (let i = 0; i < 9; i++) {
    sum += parseInt(cpf.charAt(i)) * (10 - i);
  }
  let check1 = (sum * 10) % 11;
  if (check1 === 10) check1 = 0;
  if (check1 !== parseInt(cpf.charAt(9))) return { customMessage: "CPF inválido" };

  sum = 0;
  for (let i = 0; i < 10; i++) {
    sum += parseInt(cpf.charAt(i)) * (11 - i);
  }
  let check2 = (sum * 10) % 11;
  if (check2 === 10) check2 = 0;
  if (check2 !== parseInt(cpf.charAt(10))) return { customMessage: "CPF inválido" };

  return null; // válido
}
