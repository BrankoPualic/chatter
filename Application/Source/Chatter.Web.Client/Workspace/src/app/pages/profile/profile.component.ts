import { Component, OnInit } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { api } from '../../_generated/project';
import { BaseComponentGeneric } from '../../base/base.component';
import { ErrorService } from '../../services/error.service';
import { PageLoaderService } from '../../services/page-loader.service';
import { ToastService } from '../../services/toast.service';
import { ActivatedRoute } from '@angular/router';
import { GLOBAL_MODULES } from '../../_global.modules';

@Component({
  selector: 'app-profile',
  imports: [GLOBAL_MODULES, MobileNavigationBarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent extends BaseComponentGeneric<api.UserDto> implements OnInit {
  user?: api.UserDto;
  userId: string;

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    private route: ActivatedRoute,
    private api_UserController: api.Controller.UserController
  ) {
    super(errorService, loaderService, toastService)

    route.paramMap.subscribe(params => { this.userId = params['get']('id')! });
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading = true;
    this.api_UserController.GetProfile(this.userId).toPromise()
      .then(_ => this.user = _)
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

}
