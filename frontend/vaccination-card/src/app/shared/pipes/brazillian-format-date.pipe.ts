import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'brDate'
})
export class BrazillianFormatDate implements PipeTransform {

  transform(value: string | null | undefined): string {
    if (!value) return '';

    // Supondo que value seja 'yyyy-MM-dd'
    const parts = value.split('-');
    if (parts.length !== 3) return value; // retorna original se não for válido

    const [year, month, day] = parts;
    return `${day}/${month}/${year}`;
  }

}
