import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-hello',
  standalone: true,                  // important for standalone component
  imports: [CommonModule],           // import CommonModule for directives like *ngIf, *ngFor
  templateUrl: './hello.component.html',
  styleUrls: ['./hello.component.css']
})
export class HelloComponent {}
