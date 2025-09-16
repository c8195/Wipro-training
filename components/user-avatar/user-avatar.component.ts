// src/app/shared/components/user-avatar/user-avatar.component.ts
import { Component, Input, OnInit } from '@angular/core';

export interface AvatarUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  roles?: string[];
  reputation?: number;
  profilePicture?: string;
  isOnline?: boolean;
  lastSeen?: string;
  joinedAt?: string;
  bio?: string;
  location?: string;
  website?: string;
}

@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.css']
})
export class UserAvatarComponent implements OnInit {


  @Input() user!: AvatarUser;
  @Input() size: 'xs' | 'sm' | 'md' | 'lg' | 'xl' = 'md';
  @Input() showName = true;
  @Input() showReputation = true;
  @Input() showOnlineStatus = false;
  @Input() clickable = false;
  @Input() showRole = false;
  @Input() layout: 'horizontal' | 'vertical' = 'horizontal';

  onImageError(): void {
    this.avatarUrl = '';
    this.initials = this.getInitials();
    this.backgroundColor = this.generateColor();
  }

  avatarUrl = '';
  initials = '';
  backgroundColor = '';

  ngOnInit(): void {
    this.generateAvatar();
  }

  private generateAvatar(): void {
    if (this.user?.profilePicture && this.user.profilePicture.startsWith('http')) {
      this.avatarUrl = this.user.profilePicture;
    } else if (this.user?.profilePicture) {
      this.avatarUrl = `/api/images/${this.user.profilePicture}`;
    } else {
      this.initials = this.getInitials();
      this.backgroundColor = this.generateColor();
    }
  }

  public getInitials(): string {
    if (!this.user) return 'U';
    
    const firstName = this.user.firstName || '';
    const lastName = this.user.lastName || '';
    
    if (firstName && lastName) {
      return (firstName.charAt(0) + lastName.charAt(0)).toUpperCase();
    } else if (firstName) {
      return firstName.charAt(0).toUpperCase();
    } else if (this.user.userName) {
      return this.user.userName.charAt(0).toUpperCase();
    }
    
    return 'U';
  }

  private generateColor(): string {
    const colors = [
      '#FF6B6B', '#4ECDC4', '#45B7D1', '#96CEB4', '#FECA57',
      '#FF9FF3', '#54A0FF', '#5F27CD', '#00D2D3', '#FF9F43',
      '#10AC84', '#EE5A24', '#0984e3', '#6C5CE7', '#A29BFE'
    ];
    
    const userId = this.user?.id || 0;
    return colors[userId % colors.length];
  }

  getAvatarClass(): string {
    const classes = ['user-avatar'];
    classes.push(`avatar-${this.size}`);
    if (this.clickable) classes.push('avatar-clickable');
    if (!this.avatarUrl) classes.push('avatar-initials');
    return classes.join(' ');
  }

  getContainerClass(): string {
    const classes = ['avatar-container'];
    classes.push(`layout-${this.layout}`);
    classes.push(`size-${this.size}`);
    return classes.join(' ');
  }

  getUserDisplayName(): string {
    if (!this.user) return 'Unknown User';
    return `${this.user.firstName || ''} ${this.user.lastName || ''}`.trim() || this.user.userName || 'Unknown User';
  }

  getReputationColor(): string {
    const reputation = this.user?.reputation || 0;
    if (reputation >= 1000) return 'text-success';
    if (reputation >= 500) return 'text-primary';
    if (reputation >= 100) return 'text-info';
    return 'text-muted';
  }

  getReputationIcon(): string {
    const reputation = this.user?.reputation || 0;
    if (reputation >= 1000) return 'fas fa-crown';
    if (reputation >= 500) return 'fas fa-medal';
    if (reputation >= 100) return 'fas fa-star';
    return 'fas fa-user';
  }

  getUserRole(): string {
    if (this.user?.roles?.includes('Admin')) return 'Admin';
    if (this.user?.roles?.includes('Moderator')) return 'Moderator';
    return 'Member';
  }

  getRoleClass(): string {
    const role = this.getUserRole();
    switch (role) {
      case 'Admin': return 'badge bg-danger';
      case 'Moderator': return 'badge bg-warning';
      default: return 'badge bg-secondary';
    }
  }
}