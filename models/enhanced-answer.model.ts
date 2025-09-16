
import { EnhancedUser } from './enhanced-question.model';

import { VoteStats } from './enhanced-question.model';

    export interface EnhancedAnswer {
    id: number;
    content: string;
    status: number;
    createdAt: string;
    updatedAt?: string;
    questionId: number;
    user: EnhancedUser;
   // images: ImageFile[];
    voteStats: VoteStats;
    isAccepted: boolean;
    isBestAnswer: boolean;
  }
  
  export interface CreateAnswerRequest {
    content: string;
    questionId: number;
  }