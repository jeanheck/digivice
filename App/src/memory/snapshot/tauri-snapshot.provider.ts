import { invoke } from "@tauri-apps/api/core";
import { EmulatorConnectionErrorCodes } from "@/shared/constants/emulator-connection-error-code.constant";
import { EmulatorConnectionError } from "@/memory/snapshot/emulator-connection.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import type { SnapshotProvider } from "@/memory/snapshot/snapshot.provider";

interface ReadRamError {
  code: string;
  detail: string | null;
}

export class TauriSnapshotProvider implements SnapshotProvider {
  private static isReadRamError(error: unknown): error is ReadRamError {
    if (typeof error !== "object" || error === null) {
      return false;
    }

    const { code, detail } = error as Record<string, unknown>;
    return typeof code === "string" && (typeof detail === "string" || detail === null);
  }

  private static toConnectionError(error: unknown): EmulatorConnectionError {
    if (TauriSnapshotProvider.isReadRamError(error)) {
      return new EmulatorConnectionError(error.code, error.detail);
    }

    return new EmulatorConnectionError(
      EmulatorConnectionErrorCodes.MemoryReadFailed,
      String(error),
    );
  }

  public async read(): Promise<MemorySnapshot> {
    let ram: ArrayBuffer;
    try {
      ram = await invoke<ArrayBuffer>("read_ram");
    } catch (error) {
      throw TauriSnapshotProvider.toConnectionError(error);
    }

    if (ram.byteLength !== PlayStationRamSize) {
      throw new EmulatorConnectionError(
        EmulatorConnectionErrorCodes.MemoryReadFailed,
        `read_ram returned ${ram.byteLength} bytes; expected ${PlayStationRamSize}.`,
      );
    }

    return new MemorySnapshot(ram);
  }
}
