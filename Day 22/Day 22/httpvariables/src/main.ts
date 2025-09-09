import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideHttpClient } from '@angular/common/http'; // ✅ import this

bootstrapApplication(App, {
  providers: [
    provideHttpClient() // ✅ this sets up HttpClient providers globally
  ]
}).catch(err => console.error(err));
