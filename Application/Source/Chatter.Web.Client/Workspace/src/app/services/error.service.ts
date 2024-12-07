import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IModelError } from '../models/error.model';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  private _latestErrors: IModelError[] | null = null;
  private _errorSource = new BehaviorSubject<IModelError[] | null>(null);
  errors$ = this._errorSource.asObservable();

  constructor() {
    this.errors$.subscribe(_ => this._latestErrors = _);
  }

  add(error: Record<string, string[]>): void {
    if (this._latestErrors && Array.isArray(error))
      this._latestErrors.push(...(error as IModelError[]));
    else {
      this._latestErrors = [];
      const errors = this.convertToModelError(error);
      this._latestErrors.push(...errors);
    }

    this._errorSource.next(this._latestErrors);
  }

  hasError(key: string): boolean {
    const errors = this.getErrors();
    if (errors) {
      const values = key.replace(/\s+/g, '').split(',');
      return errors?.some(e => values.some(v => e.key === v));
    }

    return false;
  }

  getErrors(): IModelError[] | null {
    return this._latestErrors;
  }

  clean(): void {
    this._errorSource.next(null);
  }

  // private
  private convertToModelError(obj: Record<string, string[]>): IModelError[] {
    return Object.entries(obj).map(([key, errors]) => ({ key, errors }));
  }
}
