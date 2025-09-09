import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch } from '@angular/common/http'; // ✅ add withFetch here
import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay
} from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  providers: [
    // Capture errors globally in browser console / error logging
    provideBrowserGlobalErrorListeners(),

    // Enable zoneless change detection (performance feature)
    provideZonelessChangeDetection(),

    // Provide routing with your defined routes
    provideRouter(routes),

    // ✅ Provide HttpClient configured to use Fetch API (for SSR + perf)
    provideHttpClient(withFetch()),

    // Enable SSR hydration and replay browser events after hydration
    provideClientHydration(withEventReplay())
  ]
};
