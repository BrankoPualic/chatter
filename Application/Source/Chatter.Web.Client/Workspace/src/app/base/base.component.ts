import { Injectable, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { Subject, takeUntil } from "rxjs";
import { Functions } from "../functions";
import { INameofOptions } from "../models/function-options.model";
import { ErrorService } from "../services/error.service";
import { PageLoaderService } from "../services/page-loader.service";
import { ToastService } from "../services/toast.service";
import { BaseConstants, IBaseComponent } from "../models/base-component.model";

@Injectable()
export abstract class BaseComponent extends BaseConstants implements IBaseComponent, OnDestroy {
  private _loading = false;
  private _destroy$ = new Subject<void>();
  errors = [];
  hasAccess = false;

  constructor
    (
      protected errorService: ErrorService,
      protected loaderService: PageLoaderService,
      protected toastService: ToastService
    ) {
    super();
    loaderService.loaderState$.pipe(takeUntil(this._destroy$)).subscribe(_ => this._loading = _);
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this.cleanErrors();
  }

  // Loader
  get loading(): boolean {
    return this._loading;
  }
  set loading(state: boolean) {
    if (state)
      this.loaderService.show();
    else
      this.loaderService.hide();
  }

  // Functions
  clone = Functions.clone;

  // Notiifcations
  error(error: Record<string, string[]>) {
    const message = Object.values(error).flat().join('\r\n');
    this.toastService.notifyError(message);
  }

  warning(error: Record<string, string[]>) {
    const message = Object.values(error).flat().join('\r\n');
    this.toastService.notifyWarning(message);
  }
  success(message: string) {
    this.toastService.notifySuccess(message);
  }

  // Error handling
  hasError(key: string): boolean {
    return this.errorService.hasError(key);
  }

  cleanErrors(): void {
    this.errorService.clean();
  }
}

export class BaseComponentGeneric<T extends object> extends BaseComponent {
  nameof = (exp: (obj: T) => any, options?: INameofOptions) => Functions.nameof<T>(exp, options);
}
