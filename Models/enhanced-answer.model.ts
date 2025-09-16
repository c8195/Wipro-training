import { EnhancedUser, VoteStats } from './enhanced-question.model';

export interface EnhancedAnswer {
    id: number;
    content: string;
    status: number;
    createdAt: string;
    updatedAt?: string;
    questionId: number;
    user: EnhancedUser;
 
    voteStats: VoteStats;
    isAccepted: boolean;
    isBestAnswer: boolean;
  }
  
  export interface CreateAnswerRequest {
    content: string;
    questionId: number;
  }