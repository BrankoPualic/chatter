import { Routes } from '@angular/router';
import { Constants } from './constants/constants';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: Constants.ROUTE_HOME,
    title: 'Home | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/home/home.component').then(_ => _.HomeComponent)
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
  },

  {
    path: Constants.ROUTE_PROFILE + '/' + Constants.PARAM_ID,
    title: 'Profile | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/profile/profile.component').then(_ => _.ProfileComponent)
  },

  {
    path: Constants.ROUTE_EXPLORE,
    title: 'Explore | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/explore/explore.component').then(_ => _.ExploreComponent)
  }
];
