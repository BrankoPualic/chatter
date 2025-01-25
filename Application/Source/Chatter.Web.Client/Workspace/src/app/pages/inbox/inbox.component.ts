import { Component, OnInit } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { SearchComponent } from "../../components/search.component";
import { BaseComponent } from '../../base/base.component';
import { api } from '../../_generated/project';
import { GLOBAL_MODULES } from '../../_global.modules';
import { ErrorService } from '../../services/error.service';
import { PageLoaderService } from '../../services/page-loader.service';
import { ToastService } from '../../services/toast.service';
import { AuthService } from '../../services/auth.service';
import { SharedService } from '../../services/shared.service';

@Component({
  selector: 'app-inbox',
  imports: [MobileNavigationBarComponent, SearchComponent, GLOBAL_MODULES],
  templateUrl: './inbox.component.html',
  styleUrl: './inbox.component.scss'
})
export class InboxComponent extends BaseComponent implements OnInit {
  chats: api.ChatDto[] = [];
  searched: string = '';

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private sharedService: SharedService,
    private api_ChatController: api.Controller.ChatController
  ) {
    super(errorService, loaderService, toastService, authService);
  }

  ngOnInit(): void {
    this.loadChats();
  }

  loadChats(): void {
    const options = new api.ChatSearchOptions();
    options.Filter = this.searched;

    this.loading = true;
    this.api_ChatController.GetChatList(options).toPromise()
      .then(_ => this.chats = _!.Data)
      .catch(_ => this.error(_.error.Errors))
      .finally(() => this.loading = false);
  }

  onSearch($event: string) {
    this.searched = $event;
    this.loadChats();
  }

  isUnreadMessage = (chat: api.ChatDto) => !chat.IsLastMessageMine && chat.LastMessageStatusId === api.eMessageStatus.Delivered;

  loadChatPhoto = (chat: api.ChatDto) => this.sharedService.getChatPhoto(chat.IsGroup, chat.ImageUrl, chat.UserGenderId);
}
