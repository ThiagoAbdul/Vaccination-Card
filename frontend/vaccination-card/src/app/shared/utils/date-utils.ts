export function isoStringToDate(dateString: string): Date {
  return new Date(dateString);
}

export function dateToAge(date: Date): number {
  const today = new Date();
  let age = today.getFullYear() - date.getFullYear();

  // Ajusta se ainda não fez aniversário neste ano
  const monthDiff = today.getMonth() - date.getMonth();
  const dayDiff = today.getDate() - date.getDate();

  if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
    age--;
  }

  return age;
}
