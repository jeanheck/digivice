export interface HealthStatus {
  isHealthy: boolean;
  errorCode: string | null;
  errorDetail: string | null;
}
