import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subject, takeUntil, filter } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ThemeService } from '../../../core/services/theme.service';
import { User } from '../../../models/user.model';
import { Notification } from '../../../models/enhanced-question.model';

@Component({
  selector: 'app-enhanced-header',
  templateUrl: './enhanced-header.component.html',
  styleUrls: ['./enhanced-header.component.css']
})
export class EnhancedHeaderComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  notifications: Notification[] = [];
  unreadCount = 0;
  isMenuCollapsed = true;
  isNotificationsPanelOpen = false;
  currentRoute = '';
  
  private destroy$ = new Subject<void>();

  navigationItems = [
    {
      label: 'Home',
      icon: 'fas fa-home',
      route: '/',
      exact: true
    },
    {
      label: 'Questions',
      icon: 'fas fa-question-circle',
      route: '/questions',
      exact: false
    },
    {
      label: 'Ask Question',
      icon: 'fas fa-plus-circle',
      route: '/questions/ask',
      exact: false,
      requireAuth: true
    },
    {
      label: 'Admin',
      icon: 'fas fa-cog',
      route: '/admin',
      exact: false,
      requireAdmin: true
    }
  ];

  userMenuItems = [
    {
      label: 'My Profile',
      icon: 'fas fa-user',
      route: '/profile',
      divider: false
    },
    {
      label: 'My Questions',
      icon: 'fas fa-list',
      route: '/questions/my',
      divider: false
    },
    {
      label: 'My Answers',
      icon: 'fas fa-comments',
      route: '/answers/my',
      divider: false
    },
    {
      label: 'Bookmarks',
      icon: 'fas fa-bookmark',
      route: '/questions/bookmarked',
      divider: false
    },
    {
      label: 'Settings',
      icon: 'fas fa-cog',
      route: '/settings',
      divider: true
    },
    {
      label: 'Logout',
      icon: 'fas fa-sign-out-alt',
      action: 'logout',
      divider: false,
      danger: true
    }
  ];

  constructor(
    private authService: AuthService,
    private notificationService: NotificationService,
    private themeService: ThemeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.subscribeToUser();
    this.subscribeToNotifications();
    this.subscribeToRoute();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private subscribeToUser(): void {
    this.authService.currentUser$
      .pipe(takeUntil(this.destroy$))
      .subscribe(user => {
        this.currentUser = user;
        if (user) {
          this.loadNotifications();
        }
      });
  }

  private subscribeToNotifications(): void {
    this.notificationService.unreadCount$
      .pipe(takeUntil(this.destroy$))
      .subscribe(count => {
        this.unreadCount = count;
      });
  }

  private subscribeToRoute(): void {
    this.router.events
  .pipe(filter(event => event instanceof NavigationEnd), takeUntil(this.destroy$))
  .subscribe(event => {
    const navEnd = event as NavigationEnd;
    this.currentRoute = navEnd.urlAfterRedirects;
    this.isMenuCollapsed = true;
    this.isNotificationsPanelOpen = false;
  });
  }

  private loadNotifications(): void {
    this.notificationService.getNotifications(1, 10)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.notifications = response.notifications || [];
          this.unreadCount = response.unreadCount || 0;
        },
        error: (error) => {
          console.error('Error loading notifications:', error);
        }
      });
  }

  toggleMenu(): void {
    this.isMenuCollapsed = !this.isMenuCollapsed;
  }

  toggleNotifications(): void {
    this.isNotificationsPanelOpen = !this.isNotificationsPanelOpen;
    if (this.isNotificationsPanelOpen) {
      this.loadNotifications();
    }
  }

  closeNotifications(): void {
    this.isNotificationsPanelOpen = false;
  }

  markNotificationAsRead(notification: Notification): void {
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            notification.isRead = true;
            this.unreadCount = Math.max(0, this.unreadCount - 1);
          },
          error: (error) => {
            console.error('Error marking notification as read:', error);
          }
        });
    }
  }

  handleUserMenuAction(action: string): void {
    switch (action) {
      case 'logout':
        this.logout();
        break;
      default:
        break;
    }
  }

  logout(): void {
    this.authService.logout();
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  isActiveRoute(route: string, exact: boolean = false): boolean {
    if (exact) {
      return this.currentRoute === route;
    }
    return this.currentRoute.startsWith(route);
  }

  shouldShowNavItem(item: any): boolean {
    if (item.requireAuth && !this.isAuthenticated()) {
      return false;
    }
    if (item.requireAdmin && !this.isAdmin()) {
      return false;
    }
    return true;
  }

  getNotificationIcon(type: number): string {
    switch (type) {
      case 1: return 'fas fa-comment';
      case 2: return 'fas fa-thumbs-up';
      case 3: return 'fas fa-thumbs-up';
      case 4: return 'fas fa-check-circle';
      case 5: return 'fas fa-check-circle';
      case 6: return 'fas fa-user-plus';
      case 7: return 'fas fa-trophy';
      default: return 'fas fa-bell';
    }
  }

  getNotificationColor(type: number): string {
    switch (type) {
      case 1: return 'text-info';
      case 2: return 'text-success';
      case 3: return 'text-success';
      case 4: return 'text-primary';
      case 5: return 'text-primary';
      case 6: return 'text-warning';
      case 7: return 'text-warning';
      default: return 'text-secondary';
    }
  }

  getUserDisplayName(): string {
    if (!this.currentUser) return '';
    return `${this.currentUser.firstName} ${this.currentUser.lastName}`.trim() || this.currentUser.userName;
  }
}