import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentComponent } from './student/student.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, StudentComponent],
  template: `<app-student></app-student>`,
})
export class AppComponent { }
