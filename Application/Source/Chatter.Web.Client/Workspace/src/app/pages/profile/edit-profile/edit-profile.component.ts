import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { InputSwitchModule } from 'primeng/inputswitch';
import { api } from '../../../_generated/project';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { BaseFormComponent } from '../../../base/base-form.component';
import { LookupDropdownComponent } from "../../../components/dropdown/lookup-dropdown/lookup-dropdown.component";
import { RequiredFieldMarkComponent } from "../../../components/required-field-mark.component";
import { ValidationDirective } from '../../../directives/validation.directive';
import { Functions } from '../../../functions';
import { AuthService } from '../../../services/auth.service';
import { ErrorService } from '../../../services/error.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ToastService } from '../../../services/toast.service';
import { FileUploadService } from '../../../services/file-upload.service';

type FileType = 'thumbnail' | 'profile';

@Component({
  selector: 'app-edit-profile',
  imports: [GLOBAL_MODULES, ReactiveFormsModule, RequiredFieldMarkComponent, LookupDropdownComponent, ValidationDirective, InputSwitchModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss'
})
export class EditProfileComponent extends BaseFormComponent<api.UserDto> {
  user: api.UserDto;
  currentGender?: api.EnumProvider;
  thumbnailUrl: string;
  profilePhotoUrl: string;

  files: File[] = [];
  private fileMap = new Map<FileType, File>();

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private referenceService: api.Providers,
    private fileUploadService: FileUploadService,
    private api_UserController: api.Controller.UserController
  ) {
    super(errorService, loaderService, toastService, authService, fb);

    this.route.data.subscribe({
      next: _ => {
        this.user = _['profile'];
        this.thumbnailUrl = this.user.Thumbnail;
        this.profilePhotoUrl = this.user.ProfilePhoto;
        this.currentGender = Functions.getEnum(referenceService, this.Providers.Genders, _ => _.Id === this.user.GenderId);
      },
      error: _ => this.error(_.error.Errors),
    });

    this.loaderService.hide();
  }

  ngOnInit(): void {
  }

  override initializeForm(): void { }
  override submit(): void {
    this.loading = true;
    this.fileUploadService.uploadMultipart('User/UpdateProfile', this.files, this.user)
      .then(() => this.router.navigate(['/' + this.Constants.ROUTE_PROFILE, this.user.Id]))
      .catch(_ => this.errorService.add(_.error.Errors))
      .finally(() => this.loading = false);
  }

  async selectFile($event: Event, type: FileType) {
    const input = $event.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    const file = input.files[0];
    await this.changePhoto(type, file);
  }

  onGenderChange = (): void => { this.user.GenderId = this.currentGender!.Id };

  // private

  private async changePhoto(key: FileType, file: File): Promise<void> {
    if (key === 'profile')
      this.user.ProfilePhoto = file.name;
    else
      this.user.Thumbnail = file.name;

    const previousFile = this.fileMap.get(key);
    if (previousFile) {
      const fileIndex = this.files.indexOf(previousFile);
      this.files.splice(fileIndex, 1);
    }

    this.fileMap.set(key, file);
    this.files.push(file);

    if (key === 'profile')
      this.profilePhotoUrl = await Functions.readFileUrl(file);
    else
      this.thumbnailUrl = await Functions.readFileUrl(file);
  }
}
