import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { api } from '../../../_generated/project';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { BaseFormComponent } from '../../../base/base-form.component';
import { SearchComponent } from "../../../components/search.component";
import { ValidationDirective } from '../../../directives/validation.directive';
import { AuthService } from '../../../services/auth.service';
import { ErrorService } from '../../../services/error.service';
import { FileUploadService } from '../../../services/file-upload.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ProfileService } from '../../../services/profile.service';
import { ToastService } from '../../../services/toast.service';

interface IExtendedUserLightDto extends api.UserLightDto {
  IsSelected: boolean;
}

@Component({
  selector: 'app-create-group-chat',
  imports: [GLOBAL_MODULES, ReactiveFormsModule, SearchComponent, ValidationDirective],
  templateUrl: './create-group-chat.component.html',
  styleUrl: './create-group-chat.component.scss'
})
export class CreateGroupChatComponent extends BaseFormComponent<api.GroupCreateDto> implements OnInit {
  users: IExtendedUserLightDto[] = [];
  selectedUsers: IExtendedUserLightDto[] = [];
  keyword = '';

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    fb: FormBuilder,
    private router: Router,
    private profileService: ProfileService,
    private fileUploadService: FileUploadService,
    private api_UserController: api.Controller.UserController,
    private api_GroupController: api.Controller.GroupController,
  ) {
    super(errorService, loaderService, toastService, authService, fb);
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      [this.nameof(_ => _.Name)]: ['']
    })
  }

  override submit(): void {
    const data = new api.GroupCreateDto();
    data.Name = this.form.get(this.nameof(_ => _.Name))?.value;
    data.Participants = this.selectedUsers.map(_ => _.Id);

    this.loading = true;
    this.fileUploadService.uploadMultipart('Group/Create', this.files, data)
      .then(() => {
        this.toastService.notifySuccess(`Group '${data.Name}' has been created.`);
        this.router.navigateByUrl('/' + this.Constants.ROUTE_INBOX);
      })
      .catch(_ => this.errorService.add(_.error.Errors))
      .finally(() => this.loading = false);
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
          this.users = _.Data.map(_ => {
            const data: IExtendedUserLightDto = {
              ..._,
              IsSelected: this.selectedUsers.findIndex(x => x.Id === _.Id) !== -1
            };

            return data;
          });
        else
          this.users = this.selectedUsers;
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  loadProfilePhoto = (user: api.UserLightDto) => this.profileService.getProfilePhoto(user.ProfilePhoto, user.GenderId);

  select(user: IExtendedUserLightDto) {
    const selectedIndex = this.selectedUsers.findIndex(_ => _.Id === user.Id);

    if (selectedIndex === -1) {
      this.selectedUsers.push(user);
      user.IsSelected = true;
      return;
    }

    this.selectedUsers.splice(selectedIndex, 1);
    user.IsSelected = false;
  }

  files: File[] = [];
  selectFile($event: Event) {
    const input = $event?.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    this.files.push(input.files[0]);
    console.log(input.files)
  }
}
