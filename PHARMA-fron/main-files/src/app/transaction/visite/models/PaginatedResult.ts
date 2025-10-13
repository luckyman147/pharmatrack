export interface PaginatedResult<T> {
data: T[];
pageIndex: number;
pageReste: number;
count: number;
pageSize: number;
}