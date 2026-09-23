import type { HealthDTO } from "@/events/dto/health.dto";
import type { HealthStatus } from "@/models/health-status";

export class HealthConverter {
  static convert(data: unknown): HealthStatus {
    if (data === null || typeof data !== "object") {
      return {
        isHealthy: false,
        errorCode: null,
        errorDetail: null,
      };
    }

    const record = data as Record<string, unknown>;
    const dto = normalizeHealthDto(record);

    return {
      isHealthy: dto.isHealthy,
      errorCode: normalizeOptionalString(dto.errorCode),
      errorDetail: normalizeOptionalString(dto.errorDetail),
    };
  }
}

function normalizeHealthDto(record: Record<string, unknown>): HealthDTO {
  const isHealthy = readBoolean(record, "isHealthy", "IsHealthy") ?? false;
  const errorCode = readString(record, "errorCode", "ErrorCode");
  const errorDetail = readString(record, "errorDetail", "ErrorDetail");

  return {
    isHealthy,
    errorCode,
    errorDetail,
  };
}

function readBoolean(
  record: Record<string, unknown>,
  camelCaseKey: string,
  pascalCaseKey: string,
): boolean | undefined {
  const value = record[camelCaseKey] ?? record[pascalCaseKey];
  return typeof value === "boolean" ? value : undefined;
}

function readString(
  record: Record<string, unknown>,
  camelCaseKey: string,
  pascalCaseKey: string,
): string | null | undefined {
  const value = record[camelCaseKey] ?? record[pascalCaseKey];
  return typeof value === "string" ? value : undefined;
}

function normalizeOptionalString(value: string | null | undefined): string | null {
  if (value === null || value === undefined) {
    return null;
  }

  const trimmedValue = value.trim();
  return trimmedValue.length > 0 ? trimmedValue : null;
}
