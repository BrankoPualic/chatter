import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment.development';
import { AuthService } from './auth.service';
import { api } from '../_generated/project';

@Injectable({
  providedIn: 'root'
})
export class MyMessageService {
  hubUrl = environment.hub;
  hubConnection?: HubConnection;

  private _messages = signal<api.MessageDto[]>([]);
  messagesSignal = this._messages.asReadonly();

  setMessages(messages: api.MessageDto[]): void {
    this._messages.set(messages);
  }

  constructor(private authService: AuthService) { }

  createHubConnection(otherUserId: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?userId=' + otherUserId, {
        accessTokenFactory: () => this.authService.getToken()!
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(_ => console.error(_));

    this.hubConnection.on('ReceiveMessageThread', (messages: api.MessageDto[]) => {
      this._messages.set(messages.map(_ => {
        return {
          ..._,
          IsMine: this.authService.getCurrentUser().id === _.UserId
        }
      })
      );
    })

    this.hubConnection.on('NewMessage', message => {
      const messages = this.messagesSignal();
      message.IsMine = this.authService.getCurrentUser().id === message.UserId;
      this._messages.set([...messages, message]);
    })
  }

  stopHubConnection() {
    this.hubConnection?.stop();
  }

  sendMessage(data: api.MessageCreateDto) {
    return this.hubConnection?.invoke('SendMessage', data)
      .catch(_ => console.error(_));
  }
}
