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

  adjustTextareaHeight(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;
    const container = textarea.parentElement as HTMLElement;

    // Reset textarea height to calculate the new scroll height correctly
    textarea.style.height = 'auto';

    const lineHeight = parseInt(window.getComputedStyle(textarea).lineHeight || '20', 10);
    const maxHeight = this.Constants.MAX_ROWS * lineHeight;
    const scrollHeight = textarea.scrollHeight;

    // Set the new textarea height
    const newHeight = Math.min(scrollHeight, maxHeight);
    textarea.style.height = `${newHeight}px`;

    // Update the container height to match the textarea's height
    container.style.height = `${newHeight}px`;
  }

  submit(): void {
    if (!this.message)
      return;

    const data: api.MessageCreateDto = {
      ChatId: this.chatId,
      SenderId: this.currentUser.id,
      RecipientId: this.chat.UserId,
      Content: this.message,
      TypeId: this.messageType(),
      StatusId: api.eMessageStatus.Sent,
      Attachments: []
    };

    this.api_MessageController.CreateMessage(data).toPromise()
      .then()
      .catch(_ => this.error(_.error.Errors));
  }

  private messageType(): api.eMessageType {
    return api.eMessageType.Text
  }
}
