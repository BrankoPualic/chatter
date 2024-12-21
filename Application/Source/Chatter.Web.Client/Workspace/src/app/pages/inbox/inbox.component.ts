import { Component } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { SearchComponent } from "../../components/search.component";
import { BaseComponent } from '../../base/base.component';

@Component({
  selector: 'app-inbox',
  imports: [MobileNavigationBarComponent, SearchComponent],
  templateUrl: './inbox.component.html',
  styleUrl: './inbox.component.scss'
})
export class InboxComponent extends BaseComponent {
  onSearch($event: string) {
    throw new Error('Method not implemented.');
  }

}
