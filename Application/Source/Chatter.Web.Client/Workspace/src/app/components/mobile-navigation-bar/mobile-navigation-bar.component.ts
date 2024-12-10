import { Component } from '@angular/core';
import { IconConstants } from '../../constants/icon-constants';

@Component({
  selector: 'app-mobile-navigation-bar',
  imports: [],
  templateUrl: './mobile-navigation-bar.component.html',
  styleUrl: './mobile-navigation-bar.component.scss'
})
export class MobileNavigationBarComponent {
  Icons = IconConstants;
}
