import { CommonModule } from '@angular/common';
import { Component, input, OnInit } from '@angular/core';
import { BaseDropdownComponent } from '../../../base/base-dropdown.component';
import { DropdownComponent } from '../dropdown.component';
import { api } from '../../../_generated/project';

@Component({
  selector: 'app-lookup-dropdown',
  standalone: true,
  imports: [CommonModule, DropdownComponent],
  templateUrl: './lookup-dropdown.component.html',
  styleUrl: './lookup-dropdown.component.scss'
})
export class LookupDropdownComponent extends BaseDropdownComponent<api.EnumProvider> implements OnInit {
  provider = input<string>();
  options: api.EnumProvider[] = [];

  constructor(private referenceService: api.Providers) { super() }

  ngOnInit(): void {
    const methodName = `get${this.provider()}` as keyof api.Providers;

    const providerMethod = this.referenceService[methodName] as (() => api.EnumProvider[]) | undefined;
    if (typeof providerMethod === 'function')
      this.options = providerMethod();
  }

  override change() {
    this.onChangeVoid.emit();
  }
}
