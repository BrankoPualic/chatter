import { AfterViewInit, Component, effect, ElementRef, OnDestroy, viewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, Subject } from 'rxjs';
import { api } from '../../../_generated/project';
import { GLOBAL_MODULES } from '../../../_global.modules';
import { BaseComponent } from '../../../base/base.component';
import { AuthService } from '../../../services/auth.service';
import { ErrorService } from '../../../services/error.service';
import { MyMessageService } from '../../../services/message.service';
import { PageLoaderService } from '../../../services/page-loader.service';
import { PresenceService } from '../../../services/presence.service';
import { ProfileService } from '../../../services/profile.service';
import { SharedService } from '../../../services/shared.service';
import { ToastService } from '../../../services/toast.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-chat',
  imports: [GLOBAL_MODULES],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent extends BaseComponent implements OnDestroy, AfterViewInit {
  messagesContainer = viewChild<ElementRef>('messagesContainer');
  textarea = viewChild<ElementRef>('textarea');
  container = viewChild<ElementRef>('container');
  originalTextareaScrollHeight: number;
  eMessageStatus = api.eMessageStatus;

  chatId: string;
  chat: api.ChatLightDto;
  messages: api.MessageDto[] = [];
  message: string;
  userFromService?: api.UserDto;

  connectionEstablished = false;
  private _typingSubject = new Subject<string>();
  typingUser?: string;

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private profileService: ProfileService,
    private sharedService: SharedService,
    private userService: UserService,
    public presenceService: PresenceService,
    public messageService: MyMessageService,
    private api_InboxController: api.Controller.InboxController,
    private api_MessageController: api.Controller.MessageController
  ) {
    super(errorService, loaderService, toastService, authService);
    this.route.paramMap.subscribe(params => this.chatId = params['get']('id')!);

    effect(() => {
      this.userFromService = this.userService.userProfileSignal();
      if (!this.connectionEstablished)
        this.loadMessages();
      this.messages = this.messageService.messagesSignal();
      setTimeout(() => {
        this.scrollToBottom();
      }, 0);
    })

    effect(() => {
      this.typingUser = this.messageService.typingUserSignal();
      setTimeout(() => {
        this.scrollToBottom();
      }, 0);
    })

    this._typingSubject.pipe(
      debounceTime(1500)
    ).subscribe(chatId => this.messageService.stopTyping(chatId));
  }

  override ngOnDestroy(): void {
    super.ngOnDestroy();
    this.userService.setUserProfile(undefined);
    this.messageService.stopHubConnection();
  }

  ngAfterViewInit(): void {
    this.originalTextareaScrollHeight = this.textarea()!.nativeElement.scrollHeight;
  }

  loadMessages(): void {
    if (this.userFromService) {
      this.setupEmptyChat();
      return;
    }

    const options = new api.MessageSearchOptions();
    options.ChatId = this.chatId || JSON.parse(this.chat.Id);
    this.loading = true;
    this.api_InboxController.GetChat(options).toPromise()
      .then(_ => {
        this.chat = _!;

        this.messageService.setMessages(_!.Messages.Data);

        this.messageService.createHubConnection(this.chat.Id);
        this.connectionEstablished = true;

        setTimeout(() => {
          this.scrollToBottom();
        }, 0);
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  private setupEmptyChat(): void {
    this.chat = {
      Id: this.Constants.GUID_EMPTY,
      UserId: this.userFromService?.Id!,
      Name: this.userFromService?.Username!,
      IsGroup: false,
      IsMuted: false,
      ImageUrl: this.userFromService?.ProfilePhoto!,
      UserGenderId: this.userFromService?.GenderId!,
      Messages: { Total: 0, Data: [] },
      GroupChatRoleId: null!
    }
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

  adjustTextareaHeight(): void {
    // Reset textarea height to calculate the new scroll height correctly
    this.textarea()!.nativeElement.style.height = 'auto';

    const lineHeight = parseInt(window.getComputedStyle(this.textarea()!.nativeElement).lineHeight || '20', 10);
    const maxHeight = this.Constants.MAX_ROWS * lineHeight;
    const scrollHeight = this.textarea()!.nativeElement.scrollHeight;

    // Set the new textarea height
    const newHeight = Math.min(scrollHeight, maxHeight);
    this.textarea()!.nativeElement.style.height = `${newHeight}px`;

    // Update the container height to match the textarea's height
    this.container()!.nativeElement.style.height = `${newHeight}px`;
  }

  submit(): void {
    if (!this.message)
      return;

    const data: api.MessageCreateDto = {
      ChatId: this.chat.Id.isEmptyGuid() ? null! : this.chat.Id,
      SenderId: this.currentUser.id,
      RecipientId: this.chat.UserId,
      Content: this.message,
      TypeId: this.messageType(),
      StatusId: api.eMessageStatus.Sent,
      Attachments: []
    };


    if (!this.userFromService) {
      this.messageService.sendMessage(data)?.then(() => setTimeout(() => {
        this.scrollToBottom();
        this.afterMessageSent();
      }, 0));
      return;
    }

    this.api_MessageController.CreateMessage(data).toPromise()
      .then((chatId) => {
        this.chat.Id = chatId;
        this.afterMessageSent();
      })
      .catch(_ => this.error(_.error.Errors))
      .finally(() => {
        if (this.userFromService) {
          this.userService.setUserProfile(undefined);
        }
        else {
          this.loadMessages();
        }
      });
  }

  startTyping(event: Event): void {
    if (this.userFromService || !event)
      return;

    this._typingSubject.next(this.chat.Id);
    this.messageService.startTyping(this.chat.Id);
  }

  openProfile() {
    if (!this.chat.IsGroup) {
      this.router.navigateByUrl('/' + this.Constants.ROUTE_PROFILE + '/' + this.chat.UserId);
    } else {
      this.router.navigateByUrl('/' + this.Constants.ROUTE_INBOX + '/' + this.Constants.ROUTE_EDIT_GROUP_CHAT + '/' + this.chat.Id);
    }
  }

  private afterMessageSent(): void {
    this.message = '';
    this.textarea()!.nativeElement.style.height = `${this.originalTextareaScrollHeight}px`;
    this.container()!.nativeElement.style.height = `${this.originalTextareaScrollHeight}px`;
  }

  private messageType(): api.eMessageType {
    return api.eMessageType.Text
  }

  private scrollToBottom(): void {
    if (this.messagesContainer()) {
      const element = this.messagesContainer()!.nativeElement;
      element.scrollTop = element.scrollHeight;
    }
  }
}
