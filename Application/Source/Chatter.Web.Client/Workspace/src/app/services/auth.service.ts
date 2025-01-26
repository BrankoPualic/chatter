import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { StorageService } from './storage.service';
import { Constants } from '../constants/constants';
import { api } from '../_generated/project';
import { PageLoaderService } from './page-loader.service';
import { ProfileService } from './profile.service';
import { ICurrentUser } from '../models/models';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _availableToken = signal<string | null>(null);
  availableTokenSignal = this._availableToken.asReadonly();

  setTokenFromStorageIfPossible(): void {
    const token = this.getToken();
    this._availableToken.set(token);
  }

  constructor(
    private router: Router,
    private storageService: StorageService,
    private loaderService: PageLoaderService,
    private profileService: ProfileService,
    private presenceService: PresenceService,
    private api_UserController: api.Controller.UserController,
  ) { }

  signout() {
    this.storageService.remove(Constants.TOKEN);
    this.presenceService.stopHubConnection();
    this.router.navigateByUrl(`/${Constants.ROUTE_LOGIN}`);
  }

  setUser(data: api.TokenDto) {
    const token = this.getToken();

    this.storageService.set(Constants.TOKEN, data.Token);

    this.presenceService.createHubConnection(data.Token);

    if (!token) {
      this.router.navigateByUrl('/');
      this.loadCurrentUser();
    }
  }

  getCurrentUser(): ICurrentUser {
    const token = this.getToken();
    if (!token) {
      return {} as ICurrentUser;
    }

    const tokenInfo = this.getDecodedToken(token);

    return this.createCurrentUser(token, tokenInfo);
  }

  loadCurrentUser(): void {
    this.loaderService.show();
    this.api_UserController.GetCurrentUser().toPromise()
      .then(_ => {
        if (_) {
          this.profileService.setProfile(_);
        }
      })
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

  private createCurrentUser(token: string, tokenInfo: any): ICurrentUser {
    const user: ICurrentUser = {
      id: tokenInfo.ID,
      roles: [],
      email: tokenInfo.EMAIL,
      username: tokenInfo.USERNAME,
      token: token,
      tokenExpirationDate: new Date(tokenInfo.exp * 1000)
    }

    if (Array.isArray(tokenInfo.ROLES))
      tokenInfo.ROLES.forEach((_: string) => user.roles?.push(Number(_)));
    else
      user.roles?.push(Number(tokenInfo.ROLES));

    return user;
  }
}
