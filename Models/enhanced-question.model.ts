export interface EnhancedQuestion {
    id: number;
    title: string;
    content: string;
    topic: string;
    tags: string[];
    status: number;
    createdAt: string;
    updatedAt?: string;
    lastActivity?: string;
    viewCount: number;
    user: EnhancedUser;
    answerCount: number;
    
    voteStats: VoteStats;
    isBookmarked: boolean;
    isFavorited: boolean;
  }
  
  export interface EnhancedUser {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    userName: string;
    roles: string[];
    reputation: number;
    profilePicture?: string;
    isOnline?: boolean;
    lastSeen?: string;
    joinedAt?: string;
    bio?: string;
    location?: string;
    website?: string;
  }
  
  export interface VoteStats {
    upVotes: number;
    downVotes: number;
    netVotes: number;
    userVote?: VoteType;
  }
  
  export enum VoteType {
    Downvote = -1,
    Upvote = 1
  }
  
  export interface VoteRequest {
    questionId?: number;
    answerId?: number;
    type: VoteType;
  }
  
  export interface Notification {
    id: number;
    title: string;
    message: string;
    type: NotificationType;
    isRead: boolean;
    relatedQuestionId?: number;
    relatedAnswerId?: number;
    createdAt: string;
  }
  
  export enum NotificationType {
    QuestionAnswer = 1,
    QuestionVote = 2,
    AnswerVote = 3,
    QuestionApproved = 4,
    AnswerApproved = 5,
    NewFollower = 6,
    Achievement = 7
  }
  
  export interface UserProfile {
    id: number;
    userId: number;
    bio?: string;
    website?: string;
    location?: string;
    profilePicture?: string;
    reputation: number;
    joinedAt: string;
    lastSeenAt?: string;
  }