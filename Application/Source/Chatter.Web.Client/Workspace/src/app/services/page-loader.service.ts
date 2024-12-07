import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PageLoaderService {
  private _loaderSubject = new BehaviorSubject<boolean>(false);
  loaderState$ = this._loaderSubject.asObservable();

  show = (): void => { this._loaderSubject.next(true); }
  hide = (): void => { this._loaderSubject.next(false) }
}
