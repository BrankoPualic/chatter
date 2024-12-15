import { Component, OnInit } from '@angular/core';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { BaseFormComponent } from '../../../base/base-form.component';
import { api } from '../../../_generated/project';
import { ErrorService } from '../../../services/error.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ToastService } from '../../../services/toast.service';
import { ValidationDirective } from '../../../directives/validation.directive';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [GLOBAL_MODULES, ReactiveFormsModule, ValidationDirective],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent extends BaseFormComponent<api.LoginDto> implements OnInit {

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    fb: FormBuilder,
    private api_AuthController: api.Controller.AuthController
  ) {
    super(errorService, loaderService, toastService, authService, fb);
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      [this.nameof(_ => _.Username)]: [''],
      [this.nameof(_ => _.Password)]: ['']
    })
  }
  override submit(): void {
    this.loading = true;
    this.cleanErrors();

    this.api_AuthController.Login(this.form.value).toPromise()
      .then(_ => { if (_) this.authService.setUser(_) })
      .catch(_ => this.errorService.add(_.error.Errors))
      .finally(() => this.loading = false);
  }

}
