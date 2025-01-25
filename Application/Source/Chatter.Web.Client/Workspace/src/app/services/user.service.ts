import { Injectable, signal } from '@angular/core';
import { api } from '../_generated/project';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor() { }

  private _userProfile = signal<api.UserDto | undefined>(undefined);
  userProfileSignal = this._userProfile.asReadonly();

  setUserProfile(data?: api.UserDto): void {
    this._userProfile.set(data);
  }
}
