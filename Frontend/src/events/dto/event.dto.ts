export interface EventDTO<T> {
  type: string;
  timestamp: string;
  payload: T;
}
