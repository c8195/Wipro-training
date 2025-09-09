import { Component } from '@angular/core';

@Component({
  selector: 'app-student-marks',
  imports: [],
  templateUrl: './student-marks.html',
  styleUrl: './student-marks.css'
})
export class StudentMarks {
  public students=[
    {"id":1001,"name":"Abhishek","marks":90},
    {"id":1002,"name":"Aditya","marks":80},
    {"id":1003,"name":"Pratik","marks":60},
    {"id":1004,"name":"Omkar","marks":50},
    {"id":1005,"name":"Omkar","marks":90}
  ];

  constructor(){

  }

  ngOnInit(){

}
}