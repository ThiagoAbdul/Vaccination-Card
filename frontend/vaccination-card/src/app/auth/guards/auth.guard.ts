import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const token = inject(AuthService).accessToken;

  if(token) return true;

  return inject(Router).createUrlTree(["login"])
};
