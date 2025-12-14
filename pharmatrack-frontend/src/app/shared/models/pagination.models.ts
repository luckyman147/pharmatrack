// Shared pagination models used across the application

export interface PaginationRequest {
  pageIndex: number;
  pageSize: number;
}

export interface PaginationResult<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  items: T[];
  totalPages?: number;
}
