import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Functions } from '../functions';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService
  ) { }

  upload<T>(url: string, file: File, params?: any): Promise<HttpEvent<T>> {
    const formData = new FormData();
    formData.append('file', file);

    if (params) {
      Functions.formatRequestDates(params);
      Object.keys(params).forEach(key => formData.append(key, params[key]));
    }

    const req = new HttpRequest('POST', this.settingsService.createApiUrl(url), formData, {
      reportProgress: true,
      responseType: 'json'
    });

    return this.http.request<T>(req).toPromise();
  }

  uploadMultipart<T>(url: string, files: File[], model?: T): Promise<HttpEvent<T>> {
    const formData = new FormData();

    // Append all files
    files.forEach((file, index) => {
      formData.append(`files[${index}]`, file);
    });

    // Append model data
    if (model) {
      Functions.formatRequestDates(model);
      formData.append('model', Functions.toJson(model))
    }

    const req = new HttpRequest('POST', this.settingsService.createApiUrl(url), formData, {
      reportProgress: true,
      responseType: 'json'
    });

    return this.http.request<T>(req).toPromise();
  }
}
