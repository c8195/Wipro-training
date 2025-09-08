import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../../shared/shared.module';

import { AdminComponent } from './admin.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PendingQuestionsComponent } from './components/pending-questions/pending-questions.component';
import { PendingAnswersComponent } from './components/pending-answers/pending-answers.component';

import { UserManagementComponent } from './components/user-management/user-management.component';
import { AllQuestionsComponent } from './components/all-questions/all-questions.component';


@NgModule({
  declarations: [
    AdminComponent,
    DashboardComponent,
    PendingQuestionsComponent,
    PendingAnswersComponent,
    UserManagementComponent,
    AllQuestionsComponent,
 
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
