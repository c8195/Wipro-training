import { Component } from '@angular/core';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NgFor],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'] // ✅ plural & array
})
export class HomeComponent {
  students: string[] = ['John', 'Mary', 'Alex'];

  addStudent() {
    const name = prompt('Enter student name:');
    if (name) this.students.push(name);
  }

  modifyStudent() {
    const index = Number(prompt('Enter student number to modify:')) - 1;
    if (index >= 0 && index < this.students.length) {
      const newName = prompt('Enter new name:');
      if (newName) this.students[index] = newName;
    }
  }

  deleteStudent() {
    const index = Number(prompt('Enter student number to delete:')) - 1;
    if (index >= 0 && index < this.students.length) {
      this.students.splice(index, 1);
    }
  }
}
