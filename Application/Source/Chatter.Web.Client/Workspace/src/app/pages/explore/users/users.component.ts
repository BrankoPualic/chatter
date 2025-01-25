import { Component, effect, OnDestroy, OnInit } from '@angular/core';
import { api } from '../../../_generated/project';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { BaseComponentGeneric } from '../../../base/base.component';
import { AuthService } from '../../../services/auth.service';
import { ErrorService } from '../../../services/error.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ProfileService } from '../../../services/profile.service';
import { ToastService } from '../../../services/toast.service';
import { SharedService } from '../../../services/shared.service';

@Component({
  selector: 'app-users',
  imports: [GLOBAL_MODULES],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent extends BaseComponentGeneric<api.UserLightDto> implements OnDestroy {
  users: api.UserLightDto[] = [];
  keyword: string = '';

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private sharedService: SharedService,
    private profileService: ProfileService,
    private api_UserController: api.Controller.UserController,
  ) {
    super(errorService, loaderService, toastService, authService)

    effect(() => {
      this.keyword = this.sharedService.exploreSearchInputSignal();
      this.load(this.keyword);
    })
  }

  override ngOnDestroy(): void {
    super.ngOnDestroy();
    this.sharedService.setExploreSearchInput('');
  }

  load(keyword: string = ''): void {
    if (!keyword) {
      this.users = [];
      return;
    }
    this.loading = true;

    const options = new api.UserSearchOptions();
    options.Filter = keyword;
    options.Skip = 0;
    options.Take = 25;

    this.api_UserController.GetUserList(options).toPromise()
      .then(_ => {
        if (_?.Data)
          this.users = _.Data
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  loadProfilePhoto = (user: api.UserLightDto) => this.profileService.getProfilePhoto(user.ProfilePhoto, user.GenderId);

}
