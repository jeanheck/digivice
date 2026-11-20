import { describe, expect, it } from "vitest";
import { ImportantItemsProvider } from "@/memory/providers/important-items.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("ImportantItemsProvider", () => {
  describe("get", () => {
    it("should return the important items when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const importantItems = ImportantItemsProvider.get(snapshot);

      expect(importantItems).toEqual({
        treeBoots: true,
        fishingPole: true,
        asukaTrophy: false,
        sunTrophy: false,
      });
    });

    it("should read the real important items addresses when the snapshot is synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x00048db4, 0x00)
          .writeByte(0x00048db5, 0x00)
          .writeByte(0x00048dc2, 0x01)
          .writeByte(0x00048dc4, 0x01)
          .build(),
      );

      const importantItems = ImportantItemsProvider.get(snapshot);

      expect(importantItems).toEqual({
        treeBoots: false,
        fishingPole: false,
        asukaTrophy: true,
        sunTrophy: true,
      });
    });
  });
});
