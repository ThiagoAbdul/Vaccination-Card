export interface ResponseError {
  message: string;
  code?: string | null;
  details?: Record<string, any> | null;
  errors?: string[] | null;
}
