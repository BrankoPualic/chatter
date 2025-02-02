import { Injectable, signal } from '@angular/core';
import { ProfileService } from './profile.service';
import { Constants } from '../constants/constants';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  constructor(private profileService: ProfileService) { }

  private _exploreSearchInput = signal<string>('');
  exploreSearchInputSignal = this._exploreSearchInput.asReadonly();

  setExploreSearchInput(value: string): void {
    this._exploreSearchInput.set(value);
  }

  getChatPhoto(isGroup: boolean, photo?: string, genderId?: number): string {
    if (photo)
      return photo;

    return isGroup
      ? `../../assets/images/${Constants.DEFAULT_PHOTO_GROUP}`
      : this.profileService.getProfilePhoto(photo, genderId);
  }
}
