import { Component, input, output } from '@angular/core';
import { IconConstants } from '../constants/icon-constants';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [],
  template: `
  <div style="position: relative;">
    <span [class]="Icons.SEARCH + ' position-center small light-grey'" style="left: calc(1.5 * 0.75rem)"></span>
    <input
      type="text"
      [placeholder]="placeholder()"
      [class]="'form-control search-input ' + styleClass()"
      (input)="search($event)"
      style=""
    />
  </div>`,
  styles: `
  @import '../../assets/styles/_variables.scss';
  .search-input {
    padding-left: calc(1.5 * 0.75rem + 14px);
    background-color: $background-light;
    border: none;
    // box-shadow: rgba(0, 0, 0, 0.06) 0px 2px 4px 0px inset;
    &:focus {
      box-shadow: none;
    }
  }
  .position-center {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
  }`
})
export class SearchComponent {
  Icons = IconConstants;
  placeholder = input<string>('Search');
  styleClass = input<string>();
  debounceTime = input<number>(300);

  onSearch = output<string>();

  private _searchSubject = new Subject<string>();

  constructor() {
    this._searchSubject.pipe(
      debounceTime(this.debounceTime()),
      distinctUntilChanged()
    )
      .subscribe(_ => this.onSearch.emit(_));
  }

  search = (event: Event) => this._searchSubject.next((event.target as HTMLInputElement).value);
}
