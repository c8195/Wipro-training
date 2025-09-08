import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NotificationDropdownComponent } from './components/notification-dropdown/notification-dropdown.component';
import { ToastComponent } from './components/toast/toast.component';

import { HeaderComponent } from './components/header/header.component';
import { EnhancedHeaderComponent } from './components/header/enhanced-header.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoadingComponent } from './components/loading/loading.component';


import { ThemeToggleComponent } from './components/theme-toggle/theme-toggle.component';
import { VoteButtonsComponent } from './components/vote-buttons/vote-buttons.component';
import { UserAvatarComponent } from './components/user-avatar/user-avatar.component';
import { AdvancedSearchComponent } from './components/advanced-search/advanced-search.component';

// Pipes
import { TimeAgoPipe } from './pipes/time-ago.pipe';

@NgModule({
  declarations: [
    // Existing Components
    HeaderComponent,
    EnhancedHeaderComponent,
    FooterComponent,
    LoadingComponent,
    NotificationDropdownComponent,
    
    // New Enhanced Components
    ThemeToggleComponent,
    VoteButtonsComponent,
    UserAvatarComponent,
    AdvancedSearchComponent,
    ToastComponent,
    
    // Pipes
    TimeAgoPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  exports: [
    // Modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NotificationDropdownComponent,
    
    // Components
    HeaderComponent,
    EnhancedHeaderComponent,
    FooterComponent,
    LoadingComponent,
    ThemeToggleComponent,
    VoteButtonsComponent,
    UserAvatarComponent,
    AdvancedSearchComponent,
    LoadingComponent,
    ToastComponent,
    
    // Pipes
    TimeAgoPipe
  ]
})
export class SharedModule { }