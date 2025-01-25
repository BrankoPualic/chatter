import { ApplicationConfig, Provider, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { api } from './_generated/project';
import { SettingsService } from './services/settings.service';
import { ConfirmationService } from 'primeng/api';
import { provideAnimations } from '@angular/platform-browser/animations';
import { errorInterceptor } from './interceptors/error.interceptor';
import { jwtInterceptor } from './interceptors/jwt.interceptor';
import './extensions/string-extension';
import './extensions/observable-extension';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withFetch(), withInterceptors([
      errorInterceptor,
      jwtInterceptor
    ])),
    controllerProviders(),
    serviceProviders()
  ]
};

function controllerProviders(): Provider[] {
  return [
    api.Controller.AuthController,
    api.Controller.UserController,
    api.Controller.FollowController,
    api.Controller.ChatController,
    api.Controller.MessageController
  ]
}

function serviceProviders(): Provider[] {
  return [
    api.Providers,
    SettingsService,
    ConfirmationService
  ]
}