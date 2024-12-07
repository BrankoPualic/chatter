import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class QService {

  /**
   * Promises are executed concurrently, and all results are awaited at once.
   */
  all(promises: Promise<any>[]): Promise<any[]> {
    return Promise.all(promises);
  }

  /**
   * Promises are executed one at a time, waiting for each to finish before moving to the next.
   */
  sequential(promises: (() => Promise<any>)[]): Promise<any[]> {
    return promises.reduce((acc, promise) =>
      acc.then((results: any[]) =>
        promise().then((result: any) => [...results, result])),
      Promise.resolve<any[]>([]))
  }
}
