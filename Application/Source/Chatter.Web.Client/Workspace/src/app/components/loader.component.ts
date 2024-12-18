import { CommonModule } from "@angular/common";
import { Component, input, OnInit } from "@angular/core";
import { PageLoaderService } from "../services/page-loader.service";

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [CommonModule],
  template: `@if(isVisible){<div class="backdrop"><span class="loader"></span></div>}
    @if(containerLoader()){<div class="container-backdrop"><span class="loader"></span></div>}`,
  styles: `
    .backdrop {
        position: fixed;
        top: 0;
        left: 0;
        z-index: 9998;
        background-color: rgba(0,0,0,0.5);
        width: 100dvw;
        height: 100dvh;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .container-backdrop {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0,0,0,0.2);
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .loader {
        width: 48px;
        height: 48px;
        border-radius: 50%;
        display: inline-block;
        border-top: 3px solid #FFF;
        border-right: 3px solid transparent;
        box-sizing: border-box;
        animation: rotation 1s linear infinite;
    }
    @keyframes rotation {
    0% {
        transform: rotate(0deg);
    }
    100% {
        transform: rotate(360deg);
    }
    }`
})
export class LoaderComponent implements OnInit {
  containerLoader = input<boolean>(false);
  isVisible = false;

  constructor(private loaderService: PageLoaderService) { }

  ngOnInit(): void {
    this.loaderService.loaderState$.subscribe(_ => this.isVisible = _);
  }
}
