import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { StorageService } from './storage.service';
import { Constants } from '../constants/constants';
import { api } from '../_generated/project';
import { PageLoaderService } from './page-loader.service';
import { ProfileService } from './profile.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private router: Router,
    private storageService: StorageService,
    private loaderService: PageLoaderService,
    private profileService: ProfileService,
    private api_UserController: api.Controller.UserController,
  ) { }

  signout() {
    this.storageService.remove(Constants.TOKEN);
    this.router.navigateByUrl(`/${Constants.ROUTE_LOGIN}`);
  }

  setUser(data: api.TokenDto) {
    const token = this.getToken();

    this.storageService.set(Constants.TOKEN, data.Token);
    if (!token) {
      this.router.navigateByUrl('/');
      this.loadCurrentUser();
    }
  }

  loadCurrentUser(): void {
    this.loaderService.show();
    this.api_UserController.GetCurrentUser().toPromise()
      .then(_ => { if (_) this.profileService.setProfile(_) })
      .finally(() => this.loaderService.hide());
  }

  getToken(): string | null {
    const token = this.storageService.get(Constants.TOKEN);
    if (!token)
      return null;

    const decodedToken = this.getDecodedToken(token);
    if (!decodedToken.exp)
      return null;

    const currentTimestamp = new Date().getTime() / 1000;
    if (currentTimestamp >= decodedToken.exp)
      return null;

    return token;
  }

  // private

  private getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
