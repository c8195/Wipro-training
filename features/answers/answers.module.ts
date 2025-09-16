import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { AnswersRoutingModule } from './answers-routing.module';
import { MatDialogModule } from '@angular/material/dialog';

// Components
import { MyAnswersComponent } from './components/my-answers/my-answers.component';

@NgModule({
  declarations: [
    MyAnswersComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AnswersRoutingModule,
    MatDialogModule
  ]
})
export class AnswersModule { }