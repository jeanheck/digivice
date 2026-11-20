import { describe, expect, it } from "vitest";
import { CardBattleReader } from "@/memory/readers/card-battle.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

describe("CardBattleReader", () => {
  describe("read", () => {
    it("should map the card battle resource when the address is synthetic", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeInt32(0x1000, 11).build());

      const resource = CardBattleReader.read(snapshot, { id: 0x1000 });

      expect(resource).toEqual({ id: 11 });
    });

    it("should map the card battle resource when the address comes from the real definitions", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt32(0x0004b404, 11).build(),
      );

      const resource = CardBattleReader.read(
        snapshot,
        AddressesRepository.getCardBattleAddresses(),
      );

      expect(resource).toEqual({ id: 11 });
    });

    it("should read the id as signed when the RAM holds 0xFFFFFFFF", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt32(0x1000, 0xffffffff).build(),
      );

      const resource = CardBattleReader.read(snapshot, { id: 0x1000 });

      expect(resource.id).toBe(-1);
    });

    it("should throw MemoryReadError when the address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() => CardBattleReader.read(snapshot, { id: PlayStationRamSize - 2 })).toThrow(
        MemoryReadError,
      );
    });
  });
});
