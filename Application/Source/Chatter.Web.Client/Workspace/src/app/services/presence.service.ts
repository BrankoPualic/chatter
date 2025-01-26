import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment.development';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hub;
  private hubConnection?: HubConnection;

  private _onlineUsers = signal<string[]>([]);
  onlineUsersSignal = this._onlineUsers.asReadonly();

  constructor(private toast: ToastService) { }

  createHubConnection(token: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .catch(_ => console.error(_));

    this.hubConnection.on('UserIsOnline', username => {
      this.toast.notifySuccess(username + ' has connected');
    });

    this.hubConnection.on('UserIsOffline', username => {
      this.toast.notifyWarning(username + ' has disconnected');
    });

    this.hubConnection.on('GetOnlineUsers', usernames => {
      console.log(usernames);
      this._onlineUsers.set(usernames);
    })
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch(_ => console.error(_));
  }
}
