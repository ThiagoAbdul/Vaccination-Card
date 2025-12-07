import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap } from 'rxjs';

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService)

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      // Se não deu 401, apenas retorna o erro
      if (err.status !== 401) {
        throw err
      }

      // Evita loop infinito caso o refresh também dê 401
      if (req.url.includes('/refresh-token')) {
        throw err
      }

      const refreshToken = authService.refreshToken;

      if(!refreshToken)
        throw err

      // Tenta atualizar o token
      return authService.refreshTokens(refreshToken).pipe(
        switchMap(newTokens => {
          // Salva o novo token
          authService.storeTokens(newTokens.accessToken, newTokens.refreshToken)

          // Recria a requisição com o token atualizado
          const retryReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${newTokens.accessToken}`
            }
          })

          return next(retryReq);
        })
      )
    })
  )

};
