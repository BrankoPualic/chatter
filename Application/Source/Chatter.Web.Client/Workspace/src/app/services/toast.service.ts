import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private _warining = new Subject<string>();
  private _error = new Subject<string>();
  private _success = new Subject<string>();

  notifyWarning(message: string): void {
    this._warining.next(message);
  }

  notifyError(message: string): void {
    this._error.next(message);
  }

  notifySuccess(message: string): void {
    this._success.next(message);
  }

  notifyGeneralError(): void {
    this._error.next("Something went wrong.");
  }

  get warning() {
    return this._warining.asObservable();
  }
  get error() {
    return this._error.asObservable();
  }
  get success() {
    return this._success.asObservable();
  }
}
