import { HealthStatus } from "@/models";

export interface HealthDTO {
  status: HealthStatus;
  errorCode: string | null;
  errorDetail: string | null;
}
