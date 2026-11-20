import { describe, expect, it } from "vitest";
import { PlayerProvider } from "@/memory/providers/player.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("PlayerProvider", () => {
  describe("get", () => {
    it("should return the player when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const player = PlayerProvider.get(snapshot);

      expect(player).toEqual({
        bits: 42206,
        mapId: "021D",
        previousMapId: "1000",
        seabedRoute: null,
        mapVariant: null,
      });
    });

    it("should read the real player addresses when the snapshot is synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeInt32(0x00048da0, 1500)
          .writeInt16(0x0004b3f8, 0x0600)
          .writeInt16(0x0004b400, -1)
          .writeByte(0x00048d78, 3)
          .writeByte(0x00048d7a, 2)
          .build(),
      );

      const player = PlayerProvider.get(snapshot);

      expect(player).toEqual({
        bits: 1500,
        mapId: "0600",
        previousMapId: "FFFF",
        seabedRoute: 3,
        mapVariant: 2,
      });
    });
  });
});
