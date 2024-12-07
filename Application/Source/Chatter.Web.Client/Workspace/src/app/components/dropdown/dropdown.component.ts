import { CommonModule } from '@angular/common';
import { Component, ContentChild, input, model, output, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DropdownChangeEvent, DropdownModule } from 'primeng/dropdown';
import { LoaderComponent } from '../loader.component';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [DropdownModule, FormsModule, LoaderComponent, CommonModule],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss',
})
export class DropdownComponent {
  value = model<any>();
  options = input<any[]>([]);
  optionLabel = input<string>('name');
  filter = input<boolean>(false);
  filterBy = input<string>('name');
  disabled = input<boolean>(false);
  loading = input<boolean>(false);
  styleClass = input<string>();

  @ContentChild('selectedItemTemplate') selectedItemTemplate?: TemplateRef<any>;
  @ContentChild('itemTemplate') itemTemplate?: TemplateRef<any>;

  onChange = output<any>();
  onClick = output<void>();

  change = (event: DropdownChangeEvent) => this.onChange.emit(event);
  click = () => this.onClick.emit();
}
