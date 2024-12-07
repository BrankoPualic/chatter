import { Directive, ElementRef, input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';
import { ErrorService } from '../services/error.service';
import { IModelError } from '../models/error.model';

@Directive({
  selector: '[appValidation]',
  standalone: true,
})
export class ValidationDirective implements OnInit, OnDestroy {
  appValidation = input<string>();
  private _destroy$ = new Subject<void>();
  private errorElement: HTMLElement | null = null;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    private errorService: ErrorService,
  ) { }

  ngOnInit(): void {
    this.errorService.errors$.subscribe(_ => {
      this.handleErrors(_);
    });
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();

    if (this.errorElement)
      this.renderer.removeChild(this.el.nativeElement.parentNode, this.errorElement);
  }

  private handleErrors(errors: IModelError[] | null): void {
    // Remove existing error elements if any
    const existingErrors = this.el.nativeElement.querySelectorAll('.validation-feedback');
    existingErrors.forEach((element: HTMLElement) => {
      this.renderer.removeChild(this.el.nativeElement, element);
    });

    const formElements = this.el.nativeElement.querySelectorAll('input, select, textarea, p-dropdown');
    formElements.forEach((element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement) => {
      // Remove both error and valid classes if any
      this.renderer.removeClass(element, 'is-invalid');
      this.renderer.removeClass(element, 'is-valid');
    })

    if (!errors)
      return;

    const error = errors.find(_ => _.key === this.appValidation());

    formElements.forEach((element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement) => {

      // Check if the element has a value
      let hasValue = !!element.value && element.value.trim().length > 0;

      // Check if there is an error for this key
      const hasError =
        error &&
        error.errors.some(errMsg => {
          // Check if the error message relates to this specific form element
          // return errMsg.includes(element.id);
          return errMsg;
        });

      // PrimeNg dropdown
      if (element.tagName.toLowerCase() === 'p-dropdown') {
        const labelElement = element.querySelector('.p-dropdown-label');
        hasValue = !!labelElement?.textContent && labelElement?.textContent.trim().length > 0;
      }

      if (hasValue && !hasError) {
        // Apply the 'is-valid' class if there is a value and no error
        this.renderer.addClass(element, 'is-valid');
      } else if (hasError) {
        // Apply the 'is-invalid' class if there is an error
        this.renderer.addClass(element, 'is-invalid');
      }
    });

    if (error) {
      // Apply error styles to the form element
      // formElements.forEach((element: HTMLElement) => {
      //   this.renderer.addClass(element, 'is-invalid');
      // });

      // Create and style the error message elements
      error.errors.forEach(errMsg => {
        const errorElement = this.renderer.createElement('div');
        this.renderer.addClass(errorElement, 'validation-feedback');
        this.renderer.setStyle(errorElement, 'color', 'red');
        this.renderer.setStyle(errorElement, 'font-size', 'small');
        this.renderer.setStyle(errorElement, 'margin-top', '4px');
        errorElement.innerText = errMsg;

        // Append each error message element as a child to the container div
        this.renderer.appendChild(this.el.nativeElement, errorElement);
      });
    } else {
      // Remove error class from form elements if no error
      formElements.forEach((element: HTMLElement) => {
        this.renderer.removeClass(element, 'is-invalid');
      });
    }
  }
}
