import { Component } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { api } from '../../_generated/project';
import { GLOBAL_MODULES } from '../../_global.modules';
import { BaseComponent } from '../../base/base.component';
import { ValidationDirective } from '../../directives/validation.directive';
import { Functions } from '../../functions';
import { AuthService } from '../../services/auth.service';
import { ErrorService } from '../../services/error.service';
import { PageLoaderService } from '../../services/page-loader.service';
import { ToastService } from '../../services/toast.service';
import { FileUploadService } from '../../services/file-upload.service';
import { Router } from '@angular/router';

interface IUploadedFile {
  Id: string;
  Url: string;
  Name: string;
  Size: string;
  Duration?: string;
}

@Component({
  selector: 'app-create-post',
  imports: [GLOBAL_MODULES, ValidationDirective, InputSwitchModule],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent extends BaseComponent {
  uploadedFiles: IUploadedFile[] = [];
  files: File[] = [];
  private fileMap = new Map<string, File>();

  post = new api.PostDto();

  isVideoType = false;

  constructor(
    errorService: ErrorService,
    loaderService: PageLoaderService,
    toastService: ToastService,
    authService: AuthService,
    private router: Router,
    private fileUploadService: FileUploadService
  ) {
    super(errorService, loaderService, toastService, authService);
  }

  submit(): void {
    const model: api.PostEditDto = {
      Id: this.Constants.GUID_EMPTY,
      UserId: this.currentUser.id,
      Content: this.post.Content,
      IsCommentsDisabled: this.post.IsCommentsDisabled,
      TypeId: this.getPostType(),
      Media: []
    }

    this.loading = true;
    this.fileUploadService.uploadMultipart('Post/Save', this.files, model)
      .then(() => this.router.navigate([this.Constants.ROUTE_HOME]))
      .catch(_ => this.errorService.add(_.error.Errors))
      .finally(() => this.loading = false);
  }

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

  onPostTypeChange(): void {
    this.files = [];
    this.fileMap.clear();
    this.uploadedFiles = [];
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

  private getPostType(): api.ePostType {
    if (this.files.find(_ => _.isVideo()) && this.isVideoType)
      return api.ePostType.Video;
    else if (this.files.find(_ => _.isImage()) && !this.isVideoType)
      return api.ePostType.Image;
    else
      return api.ePostType.Text;
  }
}
