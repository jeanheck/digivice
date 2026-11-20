import { describe, expect, it } from "vitest";
import { NpcsProvider } from "@/memory/providers/npcs.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("NpcsProvider", () => {
  describe("get", () => {
    it("should return the NPC battles when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const npcs = NpcsProvider.get(snapshot);

      expect(npcs).toEqual(FieldCentralParkExpected.npcs);
    });

    it("should read the real NPC addresses when the snapshot is synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x4b39a, 0x02).writeByte(0x48dbb, 0x01).build(),
      );

      const npcs = NpcsProvider.get(snapshot);

      expect(npcs.natsumi.battles).toEqual([{ id: "first", won: true }]);
      expect(npcs.byakkoLeader.battles).toEqual([{ id: "first", won: true }]);
      expect(npcs.genji.battles).toEqual([
        { id: "first", won: false },
        { id: "second", won: false },
      ]);
      expect(npcs.catherine.battles).toEqual([{ id: "first", won: false }]);
    });
  });
});
