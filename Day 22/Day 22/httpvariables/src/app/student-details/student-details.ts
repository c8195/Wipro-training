// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-student-details',
//   imports: [],
//   templateUrl: './student-details.html',
//   styleUrl: './student-details.css'
// })
// export class StudentDetails {
//   // public students=[
//   //   {"id":1001,"name":"Abhishek","marks":90},
//   //   {"id":1002,"name":"Aditya","marks":80},
//   //   {"id":1003,"name":"Pratik","marks":60},
//   //   {"id":1004,"name":"Omkar","marks":50},
//   //   {"id":1005,"name":"Omkar","marks":90}
//   // ];

//   constructor(private studentService: Studentservice){

//   }

//   ngOnInit(){
//     this.studentService.getStudents().subscribe(data >= this.student = data);
// }
// }import { Component, OnInit } from '@angular/core';

import { Component, OnInit } from '@angular/core';       // ✅ Added OnInit import
import { CommonModule } from '@angular/common';
import { StudentService } from '../services/student.service'; // ✅ Ensure this path is correct

@Component({
  selector: 'app-student-details',
  standalone: true,             // ✅ Standalone so it can be imported into App
  imports: [CommonModule],      // ✅ Required for *ngFor, *ngIf, etc.
  templateUrl: './student-details.html',
  styleUrls: ['./student-details.css']
})
export class StudentDetails implements OnInit {
  students: any[] = [];

  constructor(private studentService: StudentService) {}

  ngOnInit() {
    this.studentService.getStudents().subscribe((data: any[]) => {
      this.students = data;
    });
  }
}
