import { describe, expect, it } from "vitest";
import type { NpcAddresses, NpcsAddresses } from "@/memory/addresses/npcs.addresses";
import { NpcsReader } from "@/memory/readers/npcs.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

function createAddresses(overrides: Partial<NpcsAddresses> = {}): NpcsAddresses {
  const emptyNpcs = Object.keys(AddressesRepository.getNpcsAddresses()).map((name) => [
    name,
    { battles: {} } satisfies NpcAddresses,
  ]);

  return { ...(Object.fromEntries(emptyNpcs) as NpcsAddresses), ...overrides };
}

function singleBattle(address: number, bitMask: number): NpcAddresses {
  return { battles: { first: { address, bitMask } } };
}

describe("NpcsReader", () => {
  describe("read", () => {
    it("should extract the battle value from the address and bit mask", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x4b39a, 0x7b).build(),
      );

      const resource = NpcsReader.read(
        snapshot,
        createAddresses({ catherine: singleBattle(0x4b39a, 0x08) }),
      );

      expect(resource.catherine.battles).toEqual([{ id: "first", value: 0x08 }]);
    });

    it("should return zero when the bit mask is not set", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x4b39a, 0x0b).build(),
      );

      const resource = NpcsReader.read(
        snapshot,
        createAddresses({ lucia: singleBattle(0x4b39a, 0x10) }),
      );

      expect(resource.lucia.battles).toEqual([{ id: "first", value: 0 }]);
    });

    it.each([
      ["chris", 0x4b39b, 0x02, 0x03, 0x02],
      ["nakano", 0x4b39c, 0x01, 0x01, 0x01],
      ["seiryuLeader", 0x48db9, 0x01, 0x01, 0x01],
    ] as const)(
      "should read %s from its own byte",
      (name, address, bitMask, rawValue, expectedValue) => {
        const snapshot = new MemorySnapshot(
          new FakeSnapshotBuilder().writeByte(address, rawValue).build(),
        );

        const resource = NpcsReader.read(
          snapshot,
          createAddresses({ [name]: singleBattle(address, bitMask) }),
        );

        expect(resource[name].battles).toEqual([{ id: "first", value: expectedValue }]);
      },
    );

    it("should read multiple battles in JSON order when the NPC has more than one", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x4b3df, 0xa0).writeByte(0x4b39a, 0x03).build(),
      );
      const genji: NpcAddresses = {
        battles: {
          first: { address: 0x4b3df, bitMask: 0x20 },
          second: { address: 0x4b39a, bitMask: 0x01 },
        },
      };

      const resource = NpcsReader.read(snapshot, createAddresses({ genji }));

      expect(resource.genji.battles).toEqual([
        { id: "first", value: 0x20 },
        { id: "second", value: 0x01 },
      ]);
    });

    it("should return an empty list when the NPC has no battles", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      const resource = NpcsReader.read(snapshot, createAddresses());

      expect(resource.alice.battles).toEqual([]);
    });

    it("should read every NPC when the addresses come from the real definitions", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x4b3df, 0x20).writeByte(0x4b39a, 0x01).build(),
      );

      const resource = NpcsReader.read(snapshot, AddressesRepository.getNpcsAddresses());

      expect(Object.keys(resource)).toHaveLength(35);
      expect(resource.genji.battles).toEqual([
        { id: "first", value: 0x20 },
        { id: "second", value: 0x01 },
      ]);
      expect(resource.natsumi.battles).toEqual([{ id: "first", value: 0 }]);
    });

    it("should throw MemoryReadError when an address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() =>
        NpcsReader.read(snapshot, createAddresses({ bob: singleBattle(PlayStationRamSize, 0x80) })),
      ).toThrow(MemoryReadError);
    });
  });
});
