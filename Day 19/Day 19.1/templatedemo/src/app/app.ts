// import { Component, signal } from '@angular/core';
// import { RouterOutlet } from '@angular/router';

// @Component({
//   selector: 'app-root',
//   imports: [RouterOutlet],
//   template: `
//     <h1>{{ title() }}</h1>
//     <h1>{{ name }}</h1>
//     <p>Welcome to the Angular Template Demo!</p>
//   `,
//   styles: [`
//     h1 { color: blue; }
//   `] // ✅ Styles must be inside a string and wrapped in an array
// })
// export class App {
//   protected readonly title = signal('templatedemo');

//   name: string = 'Angular Template Demo';

//   constructor() {
//     // Initialization logic can go here
//   }
// }

import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule], // ✅ Needed for ngModel
  template: `
    <h1>{{ title() }}</h1>
    <h1>{{ name }}</h1>
    <p>Welcome to the Angular Template Demo!</p>

    <!-- ✅ Property Binding -->
    <img [src]="imageUrl" [alt]="name" width="200" />

    <!-- ✅ Event Binding -->
    <button (click)="changeTitle()">Change Title</button>

    <!-- ✅ Two-Way Binding -->
    <input [(ngModel)]="name" placeholder="Enter your name" />
    <p>Your name is: {{ name }}</p>
  `,
  styles: [`
    h1 { color: blue; }
    img { margin: 10px 0; }
    input { margin-top: 10px; padding: 5px; }
  `]
})
export class App {
  protected readonly title = signal('templatedemo');

  name: string = 'Angular Template Demo';
  imageUrl: string = 'https://via.placeholder.com/200';

  constructor() {
    // Initialization logic can go here
  }

  // Event binding method
  changeTitle() {
    this.title.set('Title Changed!');
  }
}
