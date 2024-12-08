import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { api } from '../_generated/project';
import { Constants } from '../constants/constants';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private _profileSource = new BehaviorSubject<api.UserDto | null>(null);
  profile$ = this._profileSource.asObservable();
  Constants = Constants;

  getProfilePhoto(photo?: string, genderId?: number): string {
    return photo || `../../../assets/images/${(genderId || 0) === api.eGender.Male
      ? Constants.DEFAULT_PHOTO_MALE
      : Constants.DEFAULT_PHOTO_FEMALE}`;
  }

  setProfile(user: api.UserDto): void {
    user.ProfileImageUrl = this.getProfilePhoto(user.ProfileImageUrl, user.Gender.Id);
    this._profileSource.next(user);
  }
}
