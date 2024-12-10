import { Component } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { IconConstants } from '../../constants/icon-constants';

@Component({
  selector: 'app-home',
  imports: [MobileNavigationBarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  Icons = IconConstants;
}
