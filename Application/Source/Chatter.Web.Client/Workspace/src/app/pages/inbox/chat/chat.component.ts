import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../base/base.component';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { ErrorService } from '../../../services/error.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { ToastService } from '../../../services/toast.service';
import { AuthService } from '../../../services/auth.service';
import { SharedService } from '../../../services/shared.service';
import { api } from '../../../_generated/project';
import { ActivatedRoute } from '@angular/router';
import { ProfileService } from '../../../services/profile.service';

@Component({
  selector: 'app-chat',
  imports: [GLOBAL_MODULES],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent extends BaseComponent implements OnInit {
  chatId: string;
  chat: api.ChatLightDto;
  messages: api.MessageDto[] = [];
  message: string;

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private route: ActivatedRoute,
    private profileService: ProfileService,
    private sharedService: SharedService,
    private api_ChatController: api.Controller.ChatController,
    private api_MessageController: api.Controller.MessageController
  ) {
    super(errorService, loaderService, toastService, authService);
    this.route.paramMap.subscribe(params => this.chatId = params['get']('id')!);
  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    const options = new api.MessageSearchOptions();
    options.ChatId = this.chatId;
    this.loading = true;
    this.api_ChatController.GetChat(options).toPromise()
      .then(_ => {
        this.chat = _!;
        this.messages = _!.Messages.Data;
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  loadChatPhoto = () => this.sharedService.getChatPhoto(this.chat.IsGroup, this.chat.ImageUrl, this.chat.UserGenderId);

  displayDate(index: number): boolean {
    if (index === 0)
      return true;

    const currentMessageDate = new Date(this.messages[index].CreatedOn).toDateString();
    const previousMessageDate = new Date(this.messages[index - 1].CreatedOn).toDateString();

    return currentMessageDate !== previousMessageDate;
  }

  displayImage = (message: api.MessageDto, index: number) => !message.IsMine && (index === 0 || (this.messages[index - 1]?.UserId !== message.UserId));

  getUserImage = (user: api.UserLightDto) => this.profileService.getProfilePhoto(user.ProfilePhoto, user.GenderId);

  indentMessage = (message: api.MessageDto, index: number) => !message.IsMine && (index !== 0 && (this.messages[index - 1]?.UserId === message.UserId));

  submit(): void {

  }
}
