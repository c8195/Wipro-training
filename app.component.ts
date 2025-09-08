import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ThemeService } from './core/services/theme.service';
import { AuthService } from './core/services/auth.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'DoConnect - Community Q&A Platform';
  isLoading = true;
  currentRoute = '';
  showScrollToTop = false;
  isMenuCollapsed = true;
  isNotificationsPanelOpen = false;
  
  private destroy$ = new Subject<void>();

  constructor(
    private themeService: ThemeService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeApp();
    this.setupRouteListener();
    this.setupScrollListener();
    
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeApp(): void {
    // Theme service is automatically initialized
    // Auth service will check for existing tokens
  }

  private setupRouteListener(): void {
    this.router.events
  .pipe(filter(e => e instanceof NavigationEnd), takeUntil(this.destroy$))
  .subscribe((event) => {
    const navEnd = event as NavigationEnd;
    this.currentRoute = navEnd.urlAfterRedirects;
    this.isMenuCollapsed  = true;
    this.isNotificationsPanelOpen = false;
  });

  }

  private setupScrollListener(): void {
    if (typeof window !== 'undefined') {
      window.addEventListener('scroll', () => {
        this.showScrollToTop = window.pageYOffset > 300;
      });
    }
  }

  scrollToTop(): void {
    if (typeof window !== 'undefined') {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  shouldShowHeader(): boolean {
    return !this.currentRoute.startsWith('/auth');
  }

  shouldShowFooter(): boolean {
    const hideFooterRoutes = ['/auth', '/questions/ask', '/admin'];
    return !hideFooterRoutes.some(route => this.currentRoute.startsWith(route));
  }
}