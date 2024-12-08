import { ApplicationConfig, Provider, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { api } from './_generated/project';
import { SettingsService } from './services/settings.service';
import { ConfirmationService } from 'primeng/api';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withFetch()),
    controllerProviders(),
    serviceProviders()
  ]
};

function controllerProviders(): Provider[] {
  return [
    api.Controller.AuthController
  ]
}

function serviceProviders(): Provider[] {
  return [
    api.Providers,
    SettingsService,
    ConfirmationService
  ]
}