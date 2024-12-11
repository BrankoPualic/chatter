import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { StorageService } from '../services/storage.service';
import { Constants } from '../constants/constants';
import { delay } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const storageService = inject(StorageService);

  const token = storageService.get(Constants.TOKEN);
  if (token)
    req = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });

  return next(req).pipe(delay(2000));
};
