import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { TabViewModule } from 'primeng/tabview';
import { GLOBAL_MODULES } from '../../_global.modules';
import { MobileNavigationBarComponent } from "../../components/mobile-navigation-bar/mobile-navigation-bar.component";
import { SearchComponent } from "../../components/search.component";
import { Constants } from '../../constants/constants';
import { SharedService } from '../../services/shared.service';

@Component({
  selector: 'app-explore',
  imports: [GLOBAL_MODULES, MobileNavigationBarComponent, SearchComponent, TabViewModule],
  templateUrl: './explore.component.html',
  styleUrl: './explore.component.scss'
})
export class ExploreComponent implements OnInit {
  activeTabIndex = 0;

  constructor(private router: Router, private route: ActivatedRoute, private sharedService: SharedService) { }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.setActiveTabIndex(event.urlAfterRedirects);
      }
    });
  }

  setActiveTabIndex(url: string): void {
    if (url.includes('/' + Constants.ROUTE_EXPLORE_USERS)) {
      this.activeTabIndex = 1;
    } else if (url.includes('/' + Constants.ROUTE_EXPLORE_POSTS)) {
      this.activeTabIndex = 2;
    } else {
      this.activeTabIndex = 0;
    }
  }

  onTabChange = (event: any) => {
    let link = '/explore';
    switch (event.index) {
      case 0:
        link = Constants.ROUTE_EXPLORE_TOP;
        break;
      case 1:
        link = Constants.ROUTE_EXPLORE_USERS;
        break;
      case 2:
        link = Constants.ROUTE_EXPLORE_POSTS
        break;
    }

    this.router.navigate([link]);
  }

  onSearch = (event: any) => this.sharedService.setExploreSearchInput(event);
}
