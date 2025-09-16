export interface Answer {
  id: number;
  content: string;
  status: number;
  createdAt: string;
  updatedAt?: string;
  questionId: number;
  user: {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    userName: string;
    roles?: string[] | undefined;
  };
  images: ImageFile[];
}

export interface CreateAnswerRequest {
  content: string;
  questionId: number;
}

export interface ImageFile {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedAt: string;
}

export interface ApproveAnswerDto {
  answerId: number;
  isApproved: boolean;
  rejectionReason?: string;
}