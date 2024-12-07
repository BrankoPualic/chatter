import { FormGroup } from "@angular/forms";
import { Constants } from "../constants/constants";
import { IconConstants } from "../constants/icon-constants";
import { DateConstants } from "../constants/date-constants";
import { IModelError } from "./models";

export interface IBaseComponent {
  errors: IModelError[];
  loading: boolean;
  hasAccess: boolean;

  hasError(key: string): boolean;
  cleanErrors(): void;
}

export interface IBaseFormComponent {
  form: FormGroup;

  initializeForm(): void;
  submit(): void;
}

export class BaseConstants {
  Constants = Constants;
  Icons = IconConstants;
  DateConstants = DateConstants;
  Providers = Constants.Providers;
}