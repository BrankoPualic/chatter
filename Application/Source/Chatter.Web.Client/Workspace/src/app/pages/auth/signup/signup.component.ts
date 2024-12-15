import { Component, OnInit } from '@angular/core';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ValidationDirective } from '../../../directives/validation.directive';
import { BaseFormComponent } from '../../../base/base-form.component';
import { api } from '../../../_generated/project';
import { ErrorService } from '../../../services/error.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ToastService } from '../../../services/toast.service';
import { AuthService } from '../../../services/auth.service';
import { RequiredFieldMarkComponent } from '../../../components/required-field-mark.component';
import { LookupDropdownComponent } from "../../../components/dropdown/lookup-dropdown/lookup-dropdown.component";

enum ePasswordVisibilityIcon {
  Password,
  ConfirmPassword
}

@Component({
  selector: 'app-signup',
  imports: [GLOBAL_MODULES, ReactiveFormsModule, ValidationDirective, RequiredFieldMarkComponent, LookupDropdownComponent],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent extends BaseFormComponent<api.SignupDto> implements OnInit {
  currentGender: api.EnumProvider;
  currentPasswordIcon = this.Icons.EYE_SLASH;
  currentConfirmPasswordIcon = this.Icons.EYE_SLASH;
  passwordVisibilityIconType = ePasswordVisibilityIcon;

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
      [this.nameof(_ => _.FirstName)]: [''],
      [this.nameof(_ => _.LastName)]: [''],
      [this.nameof(_ => _.Username)]: [''],
      [this.nameof(_ => _.Email)]: [''],
      [this.nameof(_ => _.Password)]: [''],
      [this.nameof(_ => _.ConfirmPassword)]: [''],
      [this.nameof(_ => _.GenderId)]: [0],
      [this.nameof(_ => _.IsPrivate)]: [false]
    })
  }
  override submit(): void {
    this.loading = true;
    this.cleanErrors();

    this.api_AuthController.Signup(this.form.value).toPromise()
      .then(_ => { if (_) this.authService.setUser(_) })
      .catch(_ => this.errorService.add(_.error.Errors))
      .finally(() => this.loading = false);
  }

  onGenderChange(): void {
    this.form.get(this.nameof(_ => _.GenderId))?.setValue(this.currentGender.Id);
  }

  toggleVisibility(el: HTMLInputElement, type: ePasswordVisibilityIcon): void {
    el.type = el.type === 'text' ? 'password' : 'text';

    switch (type) {
      case (ePasswordVisibilityIcon.Password):
        this.currentPasswordIcon = this.currentPasswordIcon === this.Icons.EYE_SLASH ? this.Icons.EYE : this.Icons.EYE_SLASH;
        break;
      case (ePasswordVisibilityIcon.ConfirmPassword):
        this.currentConfirmPasswordIcon = this.currentConfirmPasswordIcon === this.Icons.EYE_SLASH ? this.Icons.EYE : this.Icons.EYE_SLASH;
        break;
      default:
        break;
    }
  }
}
