import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {

  if(req.headers.has("Authorization"))
    return next(req);

  const accessToken = inject(AuthService).accessToken

  req = req.clone({ setHeaders:{
    Authorization: `Bearer ${accessToken}`
  } })

  return next(req);
};
