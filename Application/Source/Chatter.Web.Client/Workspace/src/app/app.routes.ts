import { Routes } from '@angular/router';
import { Constants } from './constants/constants';

export const routes: Routes = [
  {
    path: Constants.ROUTE_HOME,
    children: []
  },

  // Login and Register
  {
    path: Constants.ROUTE_LOGIN,
    title: 'Login | ' + Constants.TITLE,
    loadComponent: () => import('./pages/auth/login/login.component').then(_ => _.LoginComponent)
  },
];
