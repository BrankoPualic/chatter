import { Component, OnInit } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { api } from '../../_generated/project';
import { BaseComponentGeneric } from '../../base/base.component';
import { ErrorService } from '../../services/error.service';
import { PageLoaderService } from '../../services/page-loader.service';
import { ToastService } from '../../services/toast.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GLOBAL_MODULES } from '../../_global.modules';
import { ProfileService } from '../../services/profile.service';
import { QService } from '../../services/q.service';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-profile',
  imports: [GLOBAL_MODULES, MobileNavigationBarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent extends BaseComponentGeneric<api.UserDto> implements OnInit {
  user?: api.UserDto;
  userId: string;
  followData: api.FollowDto;
  isFollowed = false;

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private $q: QService,
    private route: ActivatedRoute,
    private router: Router,
    private profileService: ProfileService,
    private userService: UserService,
    private api_UserController: api.Controller.UserController,
    private api_FollowController: api.Controller.FollowController
  ) {
    super(errorService, loaderService, toastService, authService)
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.userId = params['get']('id')!;

      this.followData = {
        FollowerId: this.currentUser.id,
        FollowingId: this.userId
      };

      this.initialize();
    });
  }

  initialize(): void {
    this.loading = true;
    this.$q.sequential([
      () => this.api_UserController.GetProfile(this.userId).toPromise(),
      () => this.api_FollowController.IsFollowing(this.followData).toPromise()
    ])
      .then(result => {
        this.user = result[0];
        this.isFollowed = result[1];
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  loadProfile(): void {
    this.loading = true;
    this.api_UserController.GetProfile(this.userId).toPromise()
      .then(_ => this.user = _)
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  loadProfilePhoto = () => this.profileService.getProfilePhoto(this.user?.ProfilePhoto, this.user?.GenderId);
  loadThumbnail = () => this.profileService.getThumbnail(this.user?.Thumbnail);
  isMyProfile = () => !!this.currentUser && this.user?.Id === this.currentUser.id;

  follow = () => this.followActions(_ => _.Follow(this.followData));
  unfollow = () => this.followActions(_ => _.Unfollow(this.followData));

  followActions(func: (api: api.Controller.FollowController) => Observable<any>): void {
    this.loading = true;
    this.$q.sequential([
      () => func(this.api_FollowController).toPromise(),
      () => this.api_UserController.GetProfile(this.userId).toPromise(),
      () => this.api_FollowController.IsFollowing(this.followData).toPromise()
    ])
      .then(result => {
        this.user = result[1];
        this.isFollowed = result[2];
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  message(): void {
    this.userService.setUserProfile(this.user!);
    this.router.navigate(['/' + this.Constants.ROUTE_INBOX + '/' + this.Constants.ROUTE_CHAT, this.user?.ChatId.isEmptyGuid() ? '' : this.user?.ChatId]);
  }
}
