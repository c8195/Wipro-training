import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PendingQuestionsComponent } from './components/pending-questions/pending-questions.component';
import { PendingAnswersComponent } from './components/pending-answers/pending-answers.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { AllQuestionsComponent } from './components/all-questions/all-questions.component';
// import { AllAnswersComponent } from './components/all-answers/all-answers.Component';
import { AuthGuard } from '../../core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'pending-questions', component: PendingQuestionsComponent },
      { path: 'pending-answers', component: PendingAnswersComponent },
      { path: 'all-questions', component: AllQuestionsComponent },
    //   { path: 'all-answers', component: AllAnswersComponent },
      { path: 'users', component: UserManagementComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
