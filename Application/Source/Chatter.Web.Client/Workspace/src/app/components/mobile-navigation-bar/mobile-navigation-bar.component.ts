import { Component } from '@angular/core';
import { IconConstants } from '../../constants/icon-constants';
import { RouterModule } from '@angular/router';
import { Constants } from '../../constants/constants';
import { ICurrentUser } from '../../models/models';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-mobile-navigation-bar',
  imports: [RouterModule],
  templateUrl: './mobile-navigation-bar.component.html',
  styleUrl: './mobile-navigation-bar.component.scss'
})
export class MobileNavigationBarComponent {
  Icons = IconConstants;
  Constants = Constants;
  currentUser: ICurrentUser | null = null;

  constructor(private authService: AuthService) {
    this.currentUser = authService.getCurrentUser();
  }
}
