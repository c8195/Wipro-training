import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../../core/services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  users: any[] = [];
  loading = false;
  error = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  statusFilter = '';
  processingIds = new Set<number>();

  // Role editing modal
  showRoleModal = false;
  selectedUser: any = null;
  editingRoles: string[] = [];
  savingRoles = false;

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.error = '';

    const isActive = this.statusFilter ? this.statusFilter === 'true' : undefined;

    this.adminService.getUsers(this.currentPage, this.pageSize, isActive).subscribe({
      next: (response) => {
        this.users = response.users;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load users';
        this.loading = false;
        console.error('Error loading users:', error);
      }
    });
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadUsers();
  }

  toggleUserStatus(user: any): void {
    if (user.roles.includes('Admin')) {
      alert('Cannot modify admin user status');
      return;
    }

    this.processingIds.add(user.id);
    
    this.adminService.toggleUserStatus(user.id).subscribe({
      next: () => {
        this.processingIds.delete(user.id);
        this.loadUsers();
      },
      error: (error) => {
        this.processingIds.delete(user.id);
        this.error = 'Failed to update user status';
        console.error('Error updating user status:', error);
      }
    });
  }

  deleteUser(user: any): void {
    if (user.roles.includes('Admin')) {
      alert('Cannot delete admin users');
      return;
    }

    if (!confirm(`Are you sure you want to delete ${user.firstName} ${user.lastName}? This action cannot be undone.`)) {
      return;
    }

    this.processingIds.add(user.id);
    
    this.adminService.deleteUser(user.id).subscribe({
      next: () => {
        this.processingIds.delete(user.id);
        this.loadUsers();
      },
      error: (error) => {
        this.processingIds.delete(user.id);
        this.error = 'Failed to delete user';
        console.error('Error deleting user:', error);
      }
    });
  }

  openRoleModal(user: any): void {
    this.selectedUser = user;
    this.editingRoles = [...user.roles];
    this.showRoleModal = true;
  }

  closeRoleModal(): void {
    this.showRoleModal = false;
    this.selectedUser = null;
    this.editingRoles = [];
    this.savingRoles = false;
  }

  toggleRole(role: string, event: any): void {
    if (event.target.checked) {
      if (!this.editingRoles.includes(role)) {
        this.editingRoles.push(role);
      }
    } else {
      this.editingRoles = this.editingRoles.filter(r => r !== role);
    }
  }

  saveUserRoles(): void {
    if (!this.selectedUser || this.editingRoles.length === 0) {
      alert('User must have at least one role');
      return;
    }

    this.savingRoles = true;
    
    this.adminService.updateUserRoles(this.selectedUser.id, this.editingRoles).subscribe({
      next: () => {
        this.closeRoleModal();
        this.loadUsers();
      },
      error: (error) => {
        this.savingRoles = false;
        this.error = 'Failed to update user roles';
        console.error('Error updating user roles:', error);
      }
    });
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadUsers();
    }
  }
}
