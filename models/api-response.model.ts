export interface ApiResponse<T> {
    data: T;
    success: boolean;
    message: string;
    errors?: string[];
  }
  
  export interface PaginatedResponse<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
  }