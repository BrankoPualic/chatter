import { lastValueFrom, Observable } from 'rxjs';

declare module 'rxjs' {
  interface Observable<T> {
    toPromise<T>(this: Observable<T>): Promise<T>;
  }
}

function toPromise<T>(this: Observable<T>): Promise<T> {
  return lastValueFrom(this);
}

Observable.prototype.toPromise = toPromise;

export { };
