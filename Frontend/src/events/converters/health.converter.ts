import type { HealthDTO } from "@/events/dto/health.dto";
import { HealthStatus } from "@/models/health-status";

export class HealthConverter {
  static convert(data: unknown): HealthDTO {
    if (data === null || typeof data !== "object") {
      return {
        status: HealthStatus.Loading,
        errorCode: null,
        errorDetail: null,
      };
    }

    const record = data as Record<string, unknown>;
    return normalizeHealthDto(record);
  }
}

function normalizeHealthDto(record: Record<string, unknown>): HealthDTO {
  const status = readHealthStatus(record) ?? HealthStatus.Loading;
  const errorCode = readString(record, "errorCode", "ErrorCode");
  const errorDetail = readString(record, "errorDetail", "ErrorDetail");

  return {
    status,
    errorCode,
    errorDetail,
  };
}

function readHealthStatus(record: Record<string, unknown>): HealthStatus | undefined {
  const value = record.status ?? record.Status;
  if (typeof value !== "string") {
    return undefined;
  }

  if (value === HealthStatus.Loading || value === HealthStatus.Healthy || value === HealthStatus.Error) {
    return value;
  }

  return undefined;
}

function readString(
  record: Record<string, unknown>,
  camelCaseKey: string,
  pascalCaseKey: string,
): string | null | undefined {
  const value = record[camelCaseKey] ?? record[pascalCaseKey];
  return typeof value === "string" ? value : undefined;
}
