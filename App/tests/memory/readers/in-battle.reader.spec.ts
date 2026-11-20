import { describe, expect, it } from "vitest";
import type { InBattleAddresses } from "@/memory/addresses/parties/in-battle.addresses";
import { InBattleReader } from "@/memory/readers/in-battle.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const SyntheticAddresses: InBattleAddresses = {
  allySlotBase: 0x1000,
  slotStride: 0x20,
  condition: 0x1c,
  strength: 0x10,
  defense: 0x12,
  speed: 0x14,
  hp: { max: 0x06, current: 0x08 },
  mp: { max: 0x0a, current: 0x0c },
};

describe("InBattleReader", () => {
  describe("read", () => {
    it.each([
      [1, 0x00],
      [2, 0x20],
      [3, 0x40],
    ])(
      "should resolve the ally slot when the party slot index is %i",
      (partySlotIndex, slotOffset) => {
        const snapshot = new MemorySnapshot(
          new FakeSnapshotBuilder().writeInt16(0x1000 + slotOffset + 0x10, 252).build(),
        );

        const resource = InBattleReader.read(snapshot, SyntheticAddresses, partySlotIndex);

        expect(resource.strength).toBe(252);
      },
    );

    it("should map every field when the addresses come from the real definitions", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeInt16(0x000a4470 + 0x06, 1850)
          .writeInt16(0x000a4470 + 0x08, 1400)
          .writeInt16(0x000a4470 + 0x0a, 1140)
          .writeInt16(0x000a4470 + 0x0c, 900)
          .writeInt16(0x000a4470 + 0x10, 252)
          .writeInt16(0x000a4470 + 0x12, 185)
          .writeInt16(0x000a4470 + 0x14, 84)
          .writeByte(0x000a4470 + 0x1c, 0x04)
          .build(),
      );

      const resource = InBattleReader.read(snapshot, AddressesRepository.getInBattleAddresses(), 1);

      expect(resource).toEqual({
        condition: 0x04,
        strength: 252,
        defense: 185,
        speed: 84,
        hp: { current: 1400, max: 1850 },
        mp: { current: 900, max: 1140 },
      });
    });

    it("should read signed deltas when a debuff lowers an attribute", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt16(0x1012, -120).build(),
      );

      const resource = InBattleReader.read(snapshot, SyntheticAddresses, 1);

      expect(resource.defense).toBe(-120);
    });

    it("should throw MemoryReadError when an address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() =>
        InBattleReader.read(
          snapshot,
          { ...SyntheticAddresses, allySlotBase: PlayStationRamSize },
          1,
        ),
      ).toThrow(MemoryReadError);
    });
  });
});
