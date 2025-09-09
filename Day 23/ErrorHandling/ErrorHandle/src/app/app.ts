import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule], // ✅ Add this line
  template: `
    <h1>{{ title() }}</h1>
    <button (click)="dosomething()">Trigger Error</button>
    <p *ngIf="errorMessage" class="error-message">{{ errorMessage }}</p>
  `,
  styles: [`
    .error-message {
      color: red;
      font-weight: bold;
    }
  `]
})
export class App {
  protected readonly title = signal('error_handling_demo');
  errorMessage: string | null = null;

  dosomething() {
    try {
      // Simulating an error
      const data = JSON.parse('invalid json');
      console.log(data);
    } catch (error: any) {
      this.errorMessage = `An error occurred: ${error.message}`;
      console.error('Synchronous error is caught:', error);
    }
  }
}
