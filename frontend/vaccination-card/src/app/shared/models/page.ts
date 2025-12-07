export interface Page<T> {
  currentPage: number,
  pageSize: number,
  totalItems: number,
  totalPages: number,
  items: T[],
  hasNextPage: boolean,
  hasPreviousPage: boolean
}