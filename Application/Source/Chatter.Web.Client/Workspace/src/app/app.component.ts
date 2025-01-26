import { Component, effect, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { LoaderComponent } from './components/loader.component';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService } from 'primeng/api';
import { ToastService } from './services/toast.service';
import { PresenceService } from './services/presence.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule, RippleModule, LoaderComponent, ConfirmDialogModule],
  providers: [MessageService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(
    private messageService: MessageService,
    private toastService: ToastService,
    private presenceService: PresenceService,
    private authService: AuthService
  ) {
    authService.setTokenFromStorageIfPossible();

    effect(() => {
      const token = authService.availableTokenSignal();
      if (token)
        presenceService.createHubConnection(token);
      else
        presenceService.stopHubConnection();
    })
  }

  ngOnInit(): void {
    this.toastService.error.subscribe(_ => this.messageService.add({ severity: 'error', summary: 'Error', detail: _ }));
    this.toastService.warning.subscribe(_ => this.messageService.add({ severity: 'warn', summary: 'Warning', detail: _ }));
    this.toastService.success.subscribe(_ => this.messageService.add({ severity: 'success', summary: 'Success', detail: _ }));
  }

  ngOnDestroy(): void {
    this.presenceService.stopHubConnection();
  }
}
