import type { ConnectionDTO } from "@/events/dto/connection.dto";
import type { EmulatorConnectionStatus } from "@/models/emulator-connection-status";

export class EmulatorConnectionConverter {
    static convert(data: unknown): EmulatorConnectionStatus {
        if (data === null || typeof data !== "object") {
            return {
                isConnected: false,
                errorCode: null,
                errorDetail: null,
            };
        }

        const record = data as Record<string, unknown>;
        const dto = normalizeConnectionDto(record);

        return {
            isConnected: dto.isConnected,
            errorCode: normalizeOptionalString(dto.errorCode),
            errorDetail: normalizeOptionalString(dto.errorDetail),
        };
    }
}

function normalizeConnectionDto(record: Record<string, unknown>): ConnectionDTO {
    const isConnected = readBoolean(record, "isConnected", "IsConnected") ?? false;
    const errorCode = readString(record, "errorCode", "ErrorCode");
    const errorDetail = readString(record, "errorDetail", "ErrorDetail");

    return {
        isConnected,
        errorCode,
        errorDetail,
    };
}

function readBoolean(
    record: Record<string, unknown>,
    camelCaseKey: string,
    pascalCaseKey: string
): boolean | undefined {
    const value = record[camelCaseKey] ?? record[pascalCaseKey];
    return typeof value === "boolean" ? value : undefined;
}

function readString(
    record: Record<string, unknown>,
    camelCaseKey: string,
    pascalCaseKey: string
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
