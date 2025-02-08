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

  setProfile(user: api.UserDto): void {
    user.ProfilePhoto = this.getProfilePhoto(user.ProfilePhoto, user.Gender.Id);
    this._profileSource.next(user);
  }

  getProfilePhoto(photo?: string, genderId?: number): string {
    return photo || `../../../assets/images/${(genderId || 0) === api.eGender.Male
      ? Constants.DEFAULT_PHOTO_MALE
      : Constants.DEFAULT_PHOTO_FEMALE}`;
  }

  getThumbnail(photo?: string): string {
    return photo || `../../assets/images/default-profile-thumbnail.jpg`;
  }
}
