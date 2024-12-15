import { Component } from '@angular/core';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { SearchComponent } from "../../components/search.component";
import { TabViewModule } from 'primeng/tabview';

@Component({
  selector: 'app-explore',
  imports: [MobileNavigationBarComponent, SearchComponent, TabViewModule],
  templateUrl: './explore.component.html',
  styleUrl: './explore.component.scss'
})
export class ExploreComponent {
  activeTabIndex = 0;

  onTabChange = (event: any) => this.activeTabIndex = event
}
