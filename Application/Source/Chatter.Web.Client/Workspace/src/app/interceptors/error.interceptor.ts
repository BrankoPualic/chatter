import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastService } from '../services/toast.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);
  const router = inject(Router);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((_: HttpErrorResponse) => {
      if (_) {
        switch (_.status) {
          case 401:
            authService.signout();
            break;
          case 403:
            router.navigateByUrl('/unauthorized');
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            toast.notifyGeneralError();
            console.error(_);
            break;
          default:
            break;
        }
      }
      throw _;
    })
  )
};
