import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { VotingService } from '../../../core/services/voting.service';
import { VoteStats, VoteType } from '../../../models/enhanced-question.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-vote-buttons',
  templateUrl: './vote-buttons.component.html',
  styleUrls: ['./vote-buttons.component.css']
})
export class VoteButtonsComponent implements OnInit, OnDestroy {
  @Input() questionId?: number;
  @Input() answerId?: number;
  @Input() voteStats: VoteStats = { upVotes: 0, downVotes: 0, netVotes: 0 };
  @Input() orientation: 'vertical' | 'horizontal' = 'vertical';
  @Input() size: 'sm' | 'md' | 'lg' = 'md';
  @Output() voteChanged = new EventEmitter<VoteStats>();

  isVoting = false;
  VoteType = VoteType;
  private destroy$ = new Subject<void>();

  constructor(
    private votingService: VotingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadVoteStats();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  vote(type: VoteType): void {
    if (!this.authService.isAuthenticated() || this.isVoting) {
      return;
    }

    this.isVoting = true;

    const voteRequest = {
      questionId: this.questionId,
      answerId: this.answerId,
      type
    };

    this.votingService.vote(voteRequest)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (voteStats) => {
          this.voteStats = voteStats;
          this.voteChanged.emit(voteStats);
          this.isVoting = false;
        },
        error: (error) => {
          console.error('Error voting:', error);
          this.isVoting = false;
        }
      });
  }

  private loadVoteStats(): void {
    if (this.questionId || this.answerId) {
      this.votingService.getVoteStats(this.questionId, this.answerId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (voteStats) => {
            this.voteStats = voteStats;
          },
          error: (error) => {
            console.error('Error loading vote stats:', error);
          }
        });
    }
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  hasUserVoted(type: VoteType): boolean {
    return this.voteStats.userVote === type;
  }

  getVoteButtonClass(type: VoteType): string {
    const baseClass = `vote-btn vote-btn-${type === VoteType.Upvote ? 'up' : 'down'}`;
    const sizeClass = `vote-btn-${this.size}`;
    const activeClass = this.hasUserVoted(type) ? 'active' : '';
    const disabledClass = !this.isAuthenticated() || this.isVoting ? 'disabled' : '';
    
    return `${baseClass} ${sizeClass} ${activeClass} ${disabledClass}`.trim();
  }

  getVoteContainerClass(): string {
    return `vote-container vote-${this.orientation} vote-${this.size}`;
  }
}