import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataService } from '../data-service';

@Component({
  selector: 'app-postcomponent',
  standalone: true,
  imports: [CommonModule],
  template: `
    <ul>
      <li *ngFor="let post of posts">
        {{ post.title }}
      </li>
    </ul>
  `
})
export class Postcomponent implements OnInit {
  posts: any[] = [];

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.dataService.getPosts().subscribe((data) => {
      this.posts = data;
    });
  }
}



  // templateUrl: './postcomponent.html',
  // styleUrl: './postcomponent.css'
