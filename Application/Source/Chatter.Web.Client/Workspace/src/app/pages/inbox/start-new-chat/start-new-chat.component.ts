import { Component } from '@angular/core';
import { MobileNavigationBarComponent } from "../../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { SearchComponent } from "../../../components/search.component";
import { BaseComponent } from '../../../base/base.component';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { api } from '../../../_generated/project';
import { ErrorService } from '../../../services/error.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ToastService } from '../../../services/toast.service';
import { AuthService } from '../../../services/auth.service';
import { PresenceService } from '../../../services/presence.service';
import { ProfileService } from '../../../services/profile.service';
import { UserService } from '../../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-start-new-chat',
  imports: [MobileNavigationBarComponent, SearchComponent, GLOBAL_MODULES],
  templateUrl: './start-new-chat.component.html',
  styleUrl: './start-new-chat.component.scss'
})
export class StartNewChatComponent extends BaseComponent {
  keyword = '';
  users: api.UserLightDto[] = [];

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private router: Router,
    public presenceService: PresenceService,
    private profileService: ProfileService,
    private userService: UserService,
    private api_UserController: api.Controller.UserController
  ) {
    super(errorService, loaderService, toastService, authService);
  }

  onSearch($event: string) {
    this.keyword = $event;

    const options = new api.UserSearchOptions();
    options.Filter = this.keyword;
    options.Skip = 0;
    options.Take = 25;
    options.IsFollowed = true;

    this.loading = true;
    this.api_UserController.GetUserList(options).toPromise()
      .then(_ => {
        if (_?.Data)
          this.users = _.Data;
        else {
          this.users = [];
        }
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  loadProfilePhoto = (user: api.UserLightDto) => this.profileService.getProfilePhoto(user.ProfilePhoto, user.GenderId);

  message(user: api.UserLightDto): void {
    this.userService.setUserProfile(user as unknown as api.UserDto);
    this.router.navigate(['/' + this.Constants.ROUTE_INBOX + '/' + this.Constants.ROUTE_CHAT, '']);
  }
}
