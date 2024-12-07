import { Component, input, model, output } from "@angular/core";
import { DropdownChangeEvent } from "primeng/dropdown";

@Component({
  selector: '',
  standalone: true,
  template: '',
})
export class BaseDropdownComponent<T> {
  value = model<T>();
  disabled = input<boolean>(false);
  styleClass = input<string>();
  initialLoad = input<boolean>(false);

  onChange = output<DropdownChangeEvent>();
  onChangeVoid = output<void>();
  onClick = output<void>();


  change(event: DropdownChangeEvent) {
    this.onChange.emit(event);
  }

  click() {
    this.onClick.emit();
  }
}
