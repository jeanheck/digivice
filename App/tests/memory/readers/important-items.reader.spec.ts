import { describe, expect, it } from "vitest";
import type { ImportantItemsAddresses } from "@/memory/addresses/important-items.addresses";
import { ImportantItemsReader } from "@/memory/readers/important-items.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const SyntheticAddresses: ImportantItemsAddresses = {
  treeBoots: 0x1000,
  fishingPole: 0x1001,
  asukaTrophy: 0x2000,
  sunTrophy: 0x2002,
};

describe("ImportantItemsReader", () => {
  describe("read", () => {
    it("should map the important items resource when the addresses are synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x1000, 0x01)
          .writeByte(0x1001, 0x00)
          .writeByte(0x2000, 0x02)
          .writeByte(0x2002, 0x01)
          .build(),
      );

      const resource = ImportantItemsReader.read(snapshot, SyntheticAddresses);

      expect(resource).toEqual({
        treeBoots: 0x01,
        fishingPole: 0x00,
        asukaTrophy: 0x02,
        sunTrophy: 0x01,
      });
    });

    it("should map the important items resource when the addresses come from the real definitions", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x00048db4, 0x01)
          .writeByte(0x00048db5, 0x00)
          .writeByte(0x00048dc2, 0x01)
          .writeByte(0x00048dc4, 0x00)
          .build(),
      );

      const resource = ImportantItemsReader.read(
        snapshot,
        AddressesRepository.getImportantItemsAddresses(),
      );

      expect(resource).toEqual({
        treeBoots: 0x01,
        fishingPole: 0x00,
        asukaTrophy: 0x01,
        sunTrophy: 0x00,
      });
    });

    it("should read the raw byte when the RAM holds 0xFF", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x1000, 0xff).build(),
      );

      const resource = ImportantItemsReader.read(snapshot, SyntheticAddresses);

      expect(resource.treeBoots).toBe(0xff);
    });

    it("should throw MemoryReadError when an address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() =>
        ImportantItemsReader.read(snapshot, {
          ...SyntheticAddresses,
          sunTrophy: PlayStationRamSize,
        }),
      ).toThrow(MemoryReadError);
    });
  });
});
