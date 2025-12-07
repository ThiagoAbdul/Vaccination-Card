import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'gender',
  standalone: true
})
export class GenderPipe implements PipeTransform {

  transform(value: number | null | undefined): string {
    switch (value) {
      case 0:
        return 'Feminino';
      case 1:
        return 'Masculino';
      case 2:
        return 'Outro';
      default:
        return 'Desconhecido';
    }
  }

}
