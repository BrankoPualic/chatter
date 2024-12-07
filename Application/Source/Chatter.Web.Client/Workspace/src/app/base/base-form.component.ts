import { Router } from "@angular/router";
import { ErrorService } from "../services/error.service";
import { PageLoaderService } from "../services/page-loader.service";
import { ToastService } from "../services/toast.service";
import { BaseComponentGeneric } from "./base.component";
import { FormBuilder, FormGroup } from '@angular/forms';
import { IBaseFormComponent } from "../models/base-component.model";

/**
 * Import ReactiveFormsModule inside component imports if it's standalone component.
 */
export abstract class BaseFormComponent<T extends object> extends BaseComponentGeneric<T> implements IBaseFormComponent {
  form: FormGroup;
  formData: FormData;

  constructor
    (
      errorService: ErrorService,
      loaderService: PageLoaderService,
      toastService: ToastService,
      router: Router,
      protected fb: FormBuilder
    ) {
    super(errorService, loaderService, toastService, router);

    this.form = this.fb.group({});
    this.formData = new FormData();
  }

  abstract initializeForm(): void;

  abstract submit(): void;

  toFormData(formValueObj: T): void { }
}
