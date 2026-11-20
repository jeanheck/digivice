import { readFileSync } from "node:fs";
import { join } from "node:path";
import { fileURLToPath } from "node:url";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";

const FixturesDirectory = fileURLToPath(new URL("../fixtures/snapshots/", import.meta.url));

export class SnapshotFixtureLoader {
  public static load(name: string): ArrayBuffer {
    return SnapshotFixtureLoader.loadFile(join(FixturesDirectory, `${name}.bin`));
  }

  public static loadFile(filePath: string): ArrayBuffer {
    const fileContent = readFileSync(filePath);
    if (fileContent.byteLength !== PlayStationRamSize) {
      throw new Error(
        `Snapshot must be ${PlayStationRamSize} bytes (2 MiB PS1 RAM). Got ${fileContent.byteLength} bytes in '${filePath}'.`,
      );
    }

    // Node buffers may be views over a larger pooled allocation, so copy into a standalone ArrayBuffer.
    const snapshot = new ArrayBuffer(fileContent.byteLength);
    new Uint8Array(snapshot).set(fileContent);
    return snapshot;
  }
}
