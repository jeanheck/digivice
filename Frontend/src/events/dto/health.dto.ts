import { HealthStatus } from "@/models/health-status";

export interface HealthDTO {
  status: HealthStatus;
  errorCode: string | null;
  errorDetail: string | null;
}
