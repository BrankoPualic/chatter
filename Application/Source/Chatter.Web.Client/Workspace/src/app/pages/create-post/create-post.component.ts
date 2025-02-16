import { Component } from '@angular/core';
import { BaseComponent } from '../../base/base.component';
import { GLOBAL_MODULES } from '../../_global.modules';
import { Functions } from '../../functions';
import { ErrorService } from '../../services/error.service';
import { PageLoaderService } from '../../services/page-loader.service';
import { ToastService } from '../../services/toast.service';
import { AuthService } from '../../services/auth.service';

interface IUploadedFile {
  Id: string;
  Url: string;
  Name: string;
  Size: string;
  Duration?: string;
}

@Component({
  selector: 'app-create-post',
  imports: [GLOBAL_MODULES],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent extends BaseComponent {
  uploadedFiles: IUploadedFile[] = [];
  files: File[] = [];
  private fileMap = new Map<string, File>();

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService
  ) {
    super(errorService, loaderService, toastService, authService);
  }

  submit(): void { }

  async selectFiles($event: Event): Promise<void> {
    const input = $event?.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    const files = input.files;
    for (const file of files) {
      const isValid = await this.isValidFile(file);
      if (!isValid)
        continue;

      const uploadedFile = await this.createIUploadedFile(file);

      this.uploadedFiles.push(uploadedFile);
      this.files.push(file);
      this.fileMap.set(uploadedFile.Id, file);
    }
  }

  removeFile(file: IUploadedFile): void {
    const originalFile = this.fileMap.get(file.Id);
    if (!originalFile)
      return;

    const index = this.files.indexOf(originalFile);
    this.uploadedFiles.splice(index, 1);
    this.files.splice(index, 1);
    this.fileMap.delete(file.Id);
  }

  // private

  private async createIUploadedFile(file: File): Promise<IUploadedFile> {
    const duration = file.type.startsWith("video/") ? await Functions.getVideoDuration(file) : null;

    return {
      Id: Functions.generateId(),
      Url: await Functions.readFileUrl(file),
      Name: file.name,
      Size: Functions.formatFileSize(file.size),
      Duration: duration ? `${duration.toFixed(2)} seconds` : undefined
    }
  }

  private async isValidFile(file: File): Promise<boolean> {
    let isValid = true;
    if (file.isVideo()) {
      if (file.size > this.Constants.VIDEO_SIZE_100MB) {
        this.toastService.notifyError(`Video '${file.name.shorten()}' is too large!`);
        isValid = false;
      }

      await Functions.getVideoDuration(file)
        .then(duration => {
          if (duration > this.Constants.VIDEO_DURATION_60s) {
            this.toastService.notifyError(`Video '${file.name.shorten()}' is too long!`);
            isValid = false;
          }
        })
    }
    else if (file.isImage()) {
      if (file.size > this.Constants.IMAGE_SIZE_10MB) {
        this.toastService.notifyError(`Image '${file.name.shorten()}' is too large!`);
        isValid = false;
      }
    }

    return isValid;
  }
}
