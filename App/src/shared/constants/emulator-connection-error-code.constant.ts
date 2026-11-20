export const EmulatorConnectionErrorCodes = {
  ProcessNotFound: "process_not_found",
  MappingNotFound: "mapping_not_found",
  ConnectionFailed: "connection_failed",
  MemoryReadFailed: "memory_read_failed",
  StateComposeFailed: "state_compose_failed",
  SaveNotLoaded: "save_not_loaded",
} as const;

export type EmulatorConnectionErrorCode =
  (typeof EmulatorConnectionErrorCodes)[keyof typeof EmulatorConnectionErrorCodes];

const knownErrorCodes = new Set<string>(Object.values(EmulatorConnectionErrorCodes));

export function isKnownEmulatorConnectionErrorCode(
  value: string,
): value is EmulatorConnectionErrorCode {
  return knownErrorCodes.has(value);
}
