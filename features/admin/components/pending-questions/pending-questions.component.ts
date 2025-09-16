import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../../core/services/admin.service';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-pending-questions',
  templateUrl: './pending-questions.component.html',
  styleUrls: ['./pending-questions.component.scss']
})
export class PendingQuestionsComponent implements OnInit {
  questions: any[] = [];
  loading = false;
  error = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  processingIds = new Set<number>();

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadQuestions();
  }

  loadQuestions(): void {
    this.loading = true;
    this.error = '';

    this.adminService.getPendingQuestions(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.questions = response.questions;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load pending questions';
        this.loading = false;
        console.error('Error loading questions:', error);
      }
    });
  }

  approveQuestion(id: number): void {
    this.processingIds.add(id);
    
    this.adminService.updateQuestionStatus(id, 1).subscribe({
      next: () => {
        this.processingIds.delete(id);
        this.loadQuestions();
      },
      error: (error) => {
        this.processingIds.delete(id);
        this.error = 'Failed to approve question';
        console.error('Error approving question:', error);
      }
    });
  }

  rejectQuestion(id: number): void {
    this.processingIds.add(id);
    
    this.adminService.updateQuestionStatus(id, 2).subscribe({
      next: () => {
        this.processingIds.delete(id);
        this.loadQuestions();
      },
      error: (error) => {
        this.processingIds.delete(id);
        this.error = 'Failed to reject question';
        console.error('Error rejecting question:', error);
      }
    });
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

  getImageUrl(fileName: string): string {
    return `${environment.apiUrl}/images/${fileName}`;
  }
}
