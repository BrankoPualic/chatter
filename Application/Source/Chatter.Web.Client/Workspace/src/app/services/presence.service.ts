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

  constructor(private toastService: ToastService) { }

  createHubConnection(token: string) {
    let reconnectingCount = 0;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .catch(_ => console.error(_));

    this.hubConnection.onreconnecting(() => {
      if (reconnectingCount === 0) {
        this.toastService.notifyWarning('You are offline.');
        this._onlineUsers.set([]);
      }
    })

    this.hubConnection.onreconnected(() => {
      this.toastService.notifySuccess('You are online.');
      reconnectingCount = 0;
    })

    this.hubConnection.on('GetOnlineUsers', usernames => {
      this._onlineUsers.set(usernames);
    })
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch(_ => console.error(_));
  }
}
