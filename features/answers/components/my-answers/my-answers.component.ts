import { Component, OnInit, OnDestroy } from '@angular/core';
import { AnswerService } from '../../../../core/services/answer.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Answer } from '../../../../models/answer.model';
import { Subject, takeUntil } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-my-answers',
  templateUrl: './my-answers.component.html',
  styleUrls: ['./my-answers.component.scss']
})
export class MyAnswersComponent implements OnInit, OnDestroy {
  answers: Answer[] = [];
  loading = false;
  
  private destroy$ = new Subject<void>();

  constructor(
    private answerService: AnswerService,
    private notificationService: NotificationService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadMyAnswers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadMyAnswers(): void {
    this.loading = true;
    this.answerService.getMyAnswers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (answers) => {
          this.answers = answers;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading my answers:', error);
          this.loading = false;
        }
      });
  }

  deleteAnswer(answer: Answer): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Answer',
        message: 'Are you sure you want to delete this answer? This action cannot be undone.',
        confirmText: 'Delete',
        cancelText: 'Cancel',
        type: 'danger'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.answerService.deleteAnswer(answer.id)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.notificationService.showSuccess('Answer deleted successfully');
              this.loadMyAnswers();
            },
            error: (error) => {
              const message = error.error?.message || 'Failed to delete answer';
              this.notificationService.showError(message);
            }
          });
      }
    });
  }

  getStatusBadgeClass(status: number): string {
    switch (status) {
      case 1: return 'bg-success';
      case 0: return 'bg-warning';
      case 2: return 'bg-danger';
      default: return 'bg-secondary';
    }
  }

  getStatusText(status: number): string {
    switch (status) {
      case 1: return 'Approved';
      case 0: return 'Pending';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }

  // ADD THESE GETTER METHODS TO FIX TEMPLATE ERRORS
  get approvedAnswersCount(): number {
    return this.answers.filter(a => a.status === 1).length;
  }

  get pendingAnswersCount(): number {
    return this.answers.filter(a => a.status === 0).length;
  }

  get totalAnswersCount(): number {
    return this.answers.length;
  }
}