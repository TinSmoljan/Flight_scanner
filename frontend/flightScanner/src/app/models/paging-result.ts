export interface PagingResult<T> {
  results: T[];
  count: number;
  page: number;
  perPage: number;
  sort: string;
  sortDirection: string;
}