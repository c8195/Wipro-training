// import { Component, signal } from '@angular/core';
// import { RouterOutlet } from '@angular/router';
// import { StudentDetails } from './student-details/student-details';
// import { StudentMarks } from './student-marks/student-marks';

// @Component({
//   selector: 'app-root',
//   imports: [RouterOutlet,StudentDetails,StudentMarks],
//   templateUrl: './app.html',
//   styleUrl: './app.css'
// })
// export class App {
//   protected readonly title = signal('httpvariables');
// }import { CommonModule } from '@angular/common';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';
import { Component, signal, Pipe, PipeTransform } from '@angular/core';
import { StudentDetails } from './student-details/student-details';
import { StudentMarks } from './student-marks/student-marks';

@Pipe({
  name: 'namePipe',
  standalone: true
})
class NamePipe implements PipeTransform {
  transform(value: string, defaultValue: string): string {
    return value !== '' ? value : defaultValue;
  }
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    RouterOutlet,
    StudentDetails, // ✅ must be standalone
    StudentMarks,   // ✅ must also be standalone
    NamePipe
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('httpvariables');
}
