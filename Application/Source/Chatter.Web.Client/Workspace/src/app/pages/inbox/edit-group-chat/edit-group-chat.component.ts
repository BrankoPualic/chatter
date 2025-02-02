import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
import { Title } from '@angular/platform-browser';

interface IExtendedUserLightDto extends api.UserLightDto {
  IsSelected: boolean;
}

@Component({
  selector: 'app-edit-group-chat',
  imports: [GLOBAL_MODULES, ReactiveFormsModule, SearchComponent, ValidationDirective, FormsModule],
  templateUrl: './edit-group-chat.component.html',
  styleUrl: './edit-group-chat.component.scss'
})
export class EditGroupChatComponent extends BaseFormComponent<api.GroupEditDto> implements OnInit {
  users: IExtendedUserLightDto[] = [];
  selectedUsers: IExtendedUserLightDto[] = [];
  keyword = '';
  chatId: string;
  title: string;
  group = {} as api.GroupDto;

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private titleService: Title,
    private profileService: ProfileService,
    private fileUploadService: FileUploadService,
    private api_UserController: api.Controller.UserController,
    private api_GroupController: api.Controller.GroupController,
  ) {
    super(errorService, loaderService, toastService, authService, fb);
    this.route.paramMap.subscribe(params => {
      this.chatId = params['get']('id')!;

      this.title = this.chatId ? 'Edit Group' : 'Create Group';
      this.titleService.setTitle(this.title + ' | ' + this.Constants.TITLE);
    });
  }

  ngOnInit(): void {
    this.loadGroupChat();
  }

  isNewUser = (user: IExtendedUserLightDto) => !this.group?.Members?.find(_ => _.Id === user.Id) && user.IsSelected;

  loadGroupChat(): void {
    if (!this.chatId)
      return;

    this.loading = true;
    this.api_GroupController.GetSingle(this.chatId).toPromise()
      .then(_ => {
        this.group = _;
        this.users = _.Members.map(_ => {
          const data: IExtendedUserLightDto = {
            ..._,
            IsSelected: true,
          };
          return data;
        });
        this.selectedUsers = this.users;
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  override initializeForm(): void {
    throw new Error('Method not implemented.');
  }

  override submit(): void {
    const data = new api.GroupEditDto();
    data.Name = this.group.GroupName;
    data.Members = this.selectedUsers;
    data.Id = this.group.ChatId;

    this.loading = true;
    this.fileUploadService.uploadMultipart('Group/Save', this.files, data)
      .then(() => {
        this.toastService.notifySuccess(`Group '${data.Name}' has been ${this.chatId ? 'updated' : 'created'}.`);
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
    options.GroupId = this.chatId;
    options.IsNotPartOfGroup = true;

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

  goBack(): void {
    if (!this.chatId) {
      this.router.navigateByUrl('/' + this.Constants.ROUTE_INBOX)
    } else {
      this.router.navigateByUrl('/' + this.Constants.ROUTE_INBOX + '/' + this.Constants.ROUTE_CHAT + '/' + this.chatId);
    }
  }
}
