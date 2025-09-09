import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HelloComponent } from './example/hello/hello.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HelloComponent], // <-- Add HelloComponent here
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('my-angular-app');
}
