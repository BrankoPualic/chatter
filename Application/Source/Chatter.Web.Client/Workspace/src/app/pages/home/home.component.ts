import { Component } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";

@Component({
  selector: 'app-home',
  imports: [MobileNavigationBarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
}
