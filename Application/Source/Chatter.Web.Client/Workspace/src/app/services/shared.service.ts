import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  constructor() { }

  private _exploreSearchInput = signal<string>('');
  exploreSearchInputSignal = this._exploreSearchInput.asReadonly();

  setExploreSearchInput(value: string): void {
    this._exploreSearchInput.set(value);
  }


}
