import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../../core/services/admin.service';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-pending-answers',
  templateUrl: './pending-answers.component.html',
  styleUrls: ['./pending-answers.component.scss']
})
export class PendingAnswersComponent implements OnInit {
  answers: any[] = [];
  loading = false;
  error = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  processingIds = new Set<number>();

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadAnswers();
  }

  loadAnswers(): void {
    this.loading = true;
    this.error = '';

    this.adminService.getPendingAnswers(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.answers = response.answers;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load pending answers';
        this.loading = false;
        console.error('Error loading answers:', error);
      }
    });
  }

  approveAnswer(id: number): void {
    this.processingIds.add(id);
    
    this.adminService.updateAnswerStatus(id, 1).subscribe({
      next: () => {
        this.processingIds.delete(id);
        this.loadAnswers();
      },
      error: (error) => {
        this.processingIds.delete(id);
        this.error = 'Failed to approve answer';
        console.error('Error approving answer:', error);
      }
    });
  }

  rejectAnswer(id: number): void {
    this.processingIds.add(id);
    
    this.adminService.updateAnswerStatus(id, 2).subscribe({
      next: () => {
        this.processingIds.delete(id);
        this.loadAnswers();
      },
      error: (error) => {
        this.processingIds.delete(id);
        this.error = 'Failed to reject answer';
        console.error('Error rejecting answer:', error);
      }
    });
  }

  deleteAnswer(id: number): void {
    if (!confirm('Are you sure you want to delete this answer? This action cannot be undone.')) {
      return;
    }

    this.processingIds.add(id);
    
    this.adminService.deleteAnswer(id).subscribe({
      next: () => {
        this.processingIds.delete(id);
        this.loadAnswers();
      },
      error: (error) => {
        this.processingIds.delete(id);
        this.error = 'Failed to delete answer';
        console.error('Error deleting answer:', error);
      }
    });
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadAnswers();
    }
  }

  getImageUrl(fileName: string): string {
    return `${environment.apiUrl}/images/${fileName}`;
  }
}
    