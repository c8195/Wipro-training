import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
// import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import {Postcomponent } from './postcomponent/postcomponent';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, Postcomponent ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('fetchdataapi_demo');
}
