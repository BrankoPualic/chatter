import { cloneDeep } from "lodash";
import { INameofOptions } from "./app/models/function-options.model";

export class Functions {
  static nameof<T extends object>(
    exp: ((obj: T) => any) | (new (...params: any[]) => T),
    options?: INameofOptions,
  ): string {
    const fnStr = exp.toString();

    if (fnStr.substring(0, 6) == 'class ' && fnStr.substring(0, 8) != 'class =>') {
      return this.cleanseAssertionOperators(fnStr.substring('class '.length, fnStr.indexOf(' {')));
    }

    if (fnStr.indexOf('=>') !== -1) {
      let name = this.cleanseAssertionOperators(fnStr.substring(fnStr.indexOf('.') + 1));
      if (options?.lastPart) name = name.substring(name.lastIndexOf('.') + 1);
      return name;
    }

    throw new Error('ts-simple-nameof: Invalid function');
  }

  private static cleanseAssertionOperators(parsedName: string): string {
    return parsedName.replace(/[?!]/g, '');
  }

  // Object
  static clone = <T extends object>(model?: T): T => !!model ? cloneDeep(model) : {} as T;
}
