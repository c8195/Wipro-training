import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Core Module
import { CoreModule } from './core/core.module';

// Shared Module
import { SharedModule } from './shared/shared.module';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { ProfileModule } from './features/profile/profile.module';

@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CoreModule,
    SharedModule,
    AppRoutingModule,
    MatDialogModule,
    MatButtonModule,
    ProfileModule
  ],
 
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }