import { Component } from "@angular/core";

@Component({
  selector: 'app-required-field-mark',
  standalone: true,
  imports: [],
  template: `<span [style]="{ color: 'red' }" class="ms-1">*</span>`
})
export class RequiredFieldMarkComponent { }
