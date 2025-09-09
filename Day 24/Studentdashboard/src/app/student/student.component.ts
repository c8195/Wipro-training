import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Student {
  id: number;
  firstName: string;
  lastName: string;
  city: string;
  marks: number;
}

@Component({
  selector: 'app-student',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent {
  students: Student[] = [
    { id: 1, firstName: 'Abhi', lastName: 'Surya', city: 'Pune', marks: 85 },
    { id: 2, firstName: 'Rohit', lastName: 'Don', city: 'Mumbai', marks: 95 },
    { id: 3, firstName: 'Rohit', lastName: 'Don', city: 'Mumbai', marks: 95 },
    { id: 4, firstName: 'Rohit', lastName: 'Don', city: 'Mumbai', marks: 95 }
  ];

  newStudent: Student = { id: 0, firstName: '', lastName: '', city: '', marks: 0 };
  editIndex: number | null = null;

  addStudent() {
    if (this.editIndex === null) {
      this.newStudent.id = this.students.length + 1;
      this.students.push({ ...this.newStudent });
    } else {
      this.students[this.editIndex] = { ...this.newStudent };
      this.editIndex = null;
    }
    this.clearForm();
  }

  editStudent(index: number) {
    this.editIndex = index;
    this.newStudent = { ...this.students[index] };
  }

  deleteStudent(index: number) {
    this.students.splice(index, 1);
  }

  clearForm() {
    this.newStudent = { id: 0, firstName: '', lastName: '', city: '', marks: 0 };
    this.editIndex = null;
  }
}
