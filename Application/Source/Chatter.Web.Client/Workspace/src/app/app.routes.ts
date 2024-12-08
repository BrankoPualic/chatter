import { Routes } from '@angular/router';
import { Constants } from './constants/constants';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: Constants.ROUTE_HOME,
    canActivate: [authGuard],
    children: []
  },

  // Log in and Sign up
  {
    path: Constants.ROUTE_LOGIN,
    title: 'Log in | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/auth/login/login.component').then(_ => _.LoginComponent)
  },
  {
    path: Constants.ROUTE_SIGNUP,
    title: 'Sign up | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/auth/signup/signup.component').then(_ => _.SignupComponent)
  }
];
