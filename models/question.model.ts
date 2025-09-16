import { Answer } from "./answer.model";

export interface Question {
  id: number;
  title: string;
  content: string;
  topic: string;
  status: number;
  createdAt: string;
  updatedAt?: string;
  user: {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    userName: string;
    roles: string[];
    answers? : Answer[];
  };
  answerCount: number;
  images: ImageFile[];
}

export interface QuestionResponse {
  questions: Question[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface CreateQuestionRequest {
  title: string;
  content: string;
  topic: string;
  tags?: string[];
}
export interface ImageFile {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedAt: string;
}

export interface Topic {
  topic: string;
  count: number;
}

export enum QuestionStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2
}

export interface ApproveQuestionDto {
  questionId: number;
  isApproved: boolean;
  rejectionReason?: string;
  answers : Answer[];
}