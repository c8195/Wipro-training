import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyAnswersComponent } from './components/my-answers/my-answers.component';
import { AuthGuard } from '../../core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: MyAnswersComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnswersRoutingModule { }