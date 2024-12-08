import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { EnumNamePipe } from "./pipes/enum-name.pipe";
import { TimeAgoPipe } from "./pipes/time-ago.pipe";

export const GLOBAL_MODULES = [
  // Modules
  CommonModule,
  RouterModule,
  FormsModule,

  // Pipes
  EnumNamePipe,
  TimeAgoPipe
];
