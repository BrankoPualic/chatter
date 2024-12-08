import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  private isBrowser = false;

  constructor(@Inject(PLATFORM_ID) private platformId: object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  get(key: string): string | null {
    if (this.isBrowser)
      return localStorage.getItem(key);

    return null;
  }

  set(key: string, value: string): void {
    if (this.isBrowser)
      localStorage.setItem(key, value);
  }

  remove(key: string): void {
    if (this.isBrowser)
      localStorage.removeItem(key);
  }
}
