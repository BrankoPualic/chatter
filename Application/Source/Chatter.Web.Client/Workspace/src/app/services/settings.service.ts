import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  public ApiUrl: string = environment.api;

  public createApiUrl(path: string): string {
    return this.createUrl(this.ApiUrl, path);
  }

  private createUrl(root: string, path: string) {
    root = this.getUrl(root);

    if (path.indexOf('/') === 0) {
      path = path.substring(1);
    }

    return root + path;
  }

  private getUrl(url: string): string {
    if (url.lastIndexOf('/') !== url.length - 1) {
      url = url + '/';
    }

    return url;
  }
}
