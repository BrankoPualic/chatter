import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Constants } from '../constants/constants';
import { StorageService } from '../services/storage.service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const storageService = inject(StorageService);

  const url = state.url;
  const token = storageService.get(Constants.TOKEN);

  if (!token) {
    // No token, redirect to login if not already on login or signup
    if (!url.includes(Constants.ROUTE_LOGIN) && !url.includes(Constants.ROUTE_SIGNUP)) {
      router.navigateByUrl(`/${Constants.ROUTE_LOGIN}`);
      return false;
    }
  } else {
    // Has token, redirect to home if accessing login or signup
    if (url.includes(Constants.ROUTE_LOGIN) || url.includes(Constants.ROUTE_SIGNUP)) {
      router.navigateByUrl(`/`);
      return false;
    }
  }

  // Allow access if none of the above conditions match
  return true;
};
