import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../../core/services/admin.service';

@Component({
  selector: 'app-all-questions',
  templateUrl: './all-questions.component.html',
  styleUrls: ['./all-questions.component.scss']
})
export class AllQuestionsComponent implements OnInit {
  questions: any[] = [];
  loading = false;
  error = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  statusFilter = '';
  processingIds = new Set<number>();

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadQuestions();
  }

  loadQuestions(): void {
    this.loading = true;
    this.error = '';

    this.adminService.getAllQuestions(this.currentPage, this.pageSize, this.statusFilter || undefined).subscribe({
      next: (response) => {
        this.questions = response.questions;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load questions';
        this.loading = false;
        console.error('Error loading questions:', error);
      }
    });
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadQuestions();
  }

  deleteQuestion(id: number): void {
    if (!confirm('Are you sure you want to delete this question? This action cannot be undone.')) {
      return;
    }

    this.processingIds.add(id);
    
    this.adminService.deleteQuestion(id).subscribe({
      next: () => {
        this.processingIds.delete(id);
        this.loadQuestions();
      },
      error: (error) => {
        this.processingIds.delete(id);
        this.error = 'Failed to delete question';
        console.error('Error deleting question:', error);
      }
    });
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadQuestions();
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 0: return 'pending';
      case 1: return 'approved';
      case 2: return 'rejected';
      default: return 'pending';
    }
  }

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }
}
