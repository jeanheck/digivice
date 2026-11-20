import { mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { afterEach, beforeEach, describe, expect, it } from "vitest";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { SnapshotFixtureLoader } from "./snapshot-fixture.loader";

describe("SnapshotFixtureLoader", () => {
  let temporaryDirectory: string;

  beforeEach(() => {
    temporaryDirectory = mkdtempSync(join(tmpdir(), "digivice-snapshot-"));
  });

  afterEach(() => {
    rmSync(temporaryDirectory, { recursive: true, force: true });
  });

  describe("loadFile", () => {
    it("should return a standalone ArrayBuffer with the file content", () => {
      const content = new Uint8Array(PlayStationRamSize);
      content[0x4b3f8] = 0x00;
      content[0x4b3f9] = 0x06;
      const filePath = join(temporaryDirectory, "valid.bin");
      writeFileSync(filePath, content);

      const snapshot = SnapshotFixtureLoader.loadFile(filePath);

      expect(snapshot).toBeInstanceOf(ArrayBuffer);
      expect(snapshot.byteLength).toBe(PlayStationRamSize);
      expect(new DataView(snapshot).getInt16(0x4b3f8, true)).toBe(0x0600);
    });

    it("should throw when the file is not 2 MiB", () => {
      const filePath = join(temporaryDirectory, "truncated.bin");
      writeFileSync(filePath, new Uint8Array(1024));

      expect(() => SnapshotFixtureLoader.loadFile(filePath)).toThrow(/2 MiB/);
    });

    it("should throw when the file does not exist", () => {
      const filePath = join(temporaryDirectory, "missing.bin");

      expect(() => SnapshotFixtureLoader.loadFile(filePath)).toThrow();
    });
  });

  describe("load", () => {
    it("should throw when the fixture does not exist", () => {
      expect(() => SnapshotFixtureLoader.load("missing-fixture-name")).toThrow();
    });
  });
});
