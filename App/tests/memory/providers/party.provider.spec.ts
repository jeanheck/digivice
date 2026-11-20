import { describe, expect, it } from "vitest";
import { PartyProvider } from "@/memory/providers/party.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { BattleAgainstNatsumiExpected } from "../../fixtures/snapshots/battle-against-natsumi.expected";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("PartyProvider", () => {
  describe("get", () => {
    it("should return the three Digimons when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const party = PartyProvider.get(snapshot);

      expect(party).toEqual(FieldCentralParkExpected.party);
    });

    it("should return Kotemon in battle and two empty slots when the snapshot is the battle against Natsumi fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("battle-against-natsumi"));

      const party = PartyProvider.get(snapshot);

      expect(party).toEqual(BattleAgainstNatsumiExpected.party);
    });

    it("should return the level of the occupied slot and null ids for the empty slots", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x00048da4, 1)
          .writeByte(0x00048da8, 0xff)
          .writeByte(0x00048dac, 0xff)
          .writeInt16(0x00049878 + 0x1c, 12)
          .build(),
      );

      const party = PartyProvider.get(snapshot);

      expect(party.slots[0]).toMatchObject({ digimonId: 1, digimon: { level: 12 } });
      expect(party.slots[1]).toEqual({ index: 2, digimonId: null, digimon: null });
      expect(party.slots[2]).toEqual({ index: 3, digimonId: null, digimon: null });
    });
  });
});
