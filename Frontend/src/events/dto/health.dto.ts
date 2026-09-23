export interface HealthDTO {
  isHealthy: boolean;
  errorCode?: string | null;
  errorDetail?: string | null;
}
