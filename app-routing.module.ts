import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { profile } from 'console';
import { ProfileComponent } from './features/profile/components/profile.component';

const routes: Routes = [
  // Home Module
  { 
    path: '', 
    loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) 
  },
  
  // Auth Module
  { 
    path: 'auth', 
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) 
  },
  
  // Questions Module
  { 
    path: 'questions', 
    loadChildren: () => import('./features/questions/questions.module').then(m => m.QuestionsModule) 
  },
  

  { 
    path: 'admin', 
    loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule),
   
  },
  
 
  {   
    path: 'profile', component: ProfileComponent, canActivate : [AuthGuard]
  },
  
  // // Answers Module (Future Implementation)
  { 
    path: 'answers', 
    loadChildren: () => import('./features/answers/answers.module').then(m => m.AnswersModule),
    canActivate: [AuthGuard]
  },
  
  // // Settings Module (Future Implementation)
  { 
    path: 'settings', 
    loadChildren: () => import('./features/settings/settings.module').then(m => m.SettingsModule),
    canActivate: [AuthGuard]
  },
  
  // Wildcard Route
  { path: '**', redirectTo: '/', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { 
    enableTracing: false,
    scrollPositionRestoration: 'top',
    anchorScrolling: 'enabled',
    scrollOffset: [0, 64] // Offset for fixed header
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }