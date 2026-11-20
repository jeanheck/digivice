import { describe, expect, it } from "vitest";
import type { DigimonAddress } from "@/memory/addresses/parties/digimon.addresses";
import type { DigimonStatusAddresses } from "@/memory/addresses/parties/digimon-status.addresses";
import { DigimonReader } from "@/memory/readers/digimon.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const BlockAddress = 0x10000;
const SyntheticDigimonAddress: DigimonAddress = {
  name: "Synthetic",
  memoryBlockAddress: BlockAddress,
  blastAddress: 0x42b76,
};

const SyntheticStatusAddresses: DigimonStatusAddresses = {
  experience: 10,
  level: 14,
  tp: 16,
  hp: { current: 18, max: 20 },
  mp: { current: 22, max: 24 },
  attributes: { strength: 26, defense: 28, spirit: 30, wisdom: 32, speed: 34, charisma: 36 },
  elements: { fire: 38, water: 40, ice: 42, wind: 44, thunder: 46, machine: 48, dark: 50 },
  equipments: { head: 52, body: 54, right: 56, left: 58, accessory1: 60, accessory2: 62 },
  digievolutions: {
    activeDigievolution: 200,
    slots: [],
    unlockedDigievolutionsStart: 0x50,
    unlockedDigievolutionEntryStride: 20,
    maxUnlockedDigievolutions: 4,
    id: 0x00,
    level: 0x02,
    dvexp: 0x04,
  },
};

const InBattleAddresses = AddressesRepository.getInBattleAddresses();

function writeStoredEntry(
  builder: FakeSnapshotBuilder,
  entryIndex: number,
  digievolutionId: number,
  level: number,
  dvexp: number,
): FakeSnapshotBuilder {
  const entryAddress = BlockAddress + 0x50 + entryIndex * 20;
  return builder
    .writeInt16(entryAddress, digievolutionId)
    .writeInt16(entryAddress + 0x02, level)
    .writeInt32(entryAddress + 0x04, dvexp);
}

function readSynthetic(
  builder: FakeSnapshotBuilder,
  statusAddresses: DigimonStatusAddresses = SyntheticStatusAddresses,
  partySlotIndex = 1,
) {
  return DigimonReader.read(
    new MemorySnapshot(builder.build()),
    SyntheticDigimonAddress,
    statusAddresses,
    InBattleAddresses,
    partySlotIndex,
  );
}

function readReal(builder: FakeSnapshotBuilder, digimonId: number) {
  const digimonAddress = AddressesRepository.getDigimonAddressById(digimonId);
  if (digimonAddress === null) {
    throw new Error(`Digimon ${digimonId} not found in the definitions.`);
  }

  return DigimonReader.read(
    new MemorySnapshot(builder.build()),
    digimonAddress,
    AddressesRepository.getDigimonStatusAddresses(),
    InBattleAddresses,
    1,
  );
}

describe("DigimonReader", () => {
  describe("read", () => {
    it("should map every Digimon field when the block is fully loaded", () => {
      const builder = new FakeSnapshotBuilder()
        .writeInt32(BlockAddress + 10, 85000)
        .writeInt16(BlockAddress + 14, 45)
        .writeInt16(BlockAddress + 16, 32)
        .writeInt16(BlockAddress + 18, 500)
        .writeInt16(BlockAddress + 20, 1000)
        .writeInt16(BlockAddress + 22, 200)
        .writeInt16(BlockAddress + 24, 400)
        .writeInt16(BlockAddress + 26, 80)
        .writeInt16(BlockAddress + 28, 75)
        .writeInt16(BlockAddress + 30, 60)
        .writeInt16(BlockAddress + 32, 95)
        .writeInt16(BlockAddress + 34, 110)
        .writeInt16(BlockAddress + 36, 40)
        .writeInt16(BlockAddress + 38, 10)
        .writeInt16(BlockAddress + 40, 20)
        .writeInt16(BlockAddress + 42, 30)
        .writeInt16(BlockAddress + 44, 40)
        .writeInt16(BlockAddress + 46, 50)
        .writeInt16(BlockAddress + 48, 60)
        .writeInt16(BlockAddress + 50, 70)
        .writeInt16(BlockAddress + 52, 101)
        .writeInt16(BlockAddress + 54, 102)
        .writeInt16(BlockAddress + 56, 103)
        .writeInt16(BlockAddress + 58, 104)
        .writeInt16(BlockAddress + 60, 105)
        .writeInt16(BlockAddress + 62, 106)
        .writeInt16(BlockAddress + 200, 3)
        .writeInt16(0x42b76, 750);

      const resource = readSynthetic(builder);

      expect(resource).toMatchObject({
        activeDigievolutionId: 3,
        experience: 85000,
        level: 45,
        tp: 32,
        blast: 750,
        hp: { current: 500, max: 1000 },
        mp: { current: 200, max: 400 },
        attributes: { strength: 80, defense: 75, spirit: 60, wisdom: 95, speed: 110, charisma: 40 },
        elements: { fire: 10, water: 20, ice: 30, wind: 40, thunder: 50, machine: 60, dark: 70 },
        equipments: {
          head: 101,
          body: 102,
          right: 103,
          left: 104,
          accessory1: 105,
          accessory2: 106,
        },
        digievolutions: [],
        storedDigievolutions: [],
      });
    });

    it("should keep a zero active digievolution id as read", () => {
      const resource = readSynthetic(new FakeSnapshotBuilder());

      expect(resource.activeDigievolutionId).toBe(0);
    });

    it("should map the digievolution slots and the stored digievolutions", () => {
      const statusAddresses: DigimonStatusAddresses = {
        ...SyntheticStatusAddresses,
        digievolutions: {
          ...SyntheticStatusAddresses.digievolutions,
          slots: [
            { index: 1, address: 300 },
            { index: 2, address: 304 },
          ],
        },
      };
      let builder = new FakeSnapshotBuilder()
        .writeInt16(BlockAddress + 300, 12)
        .writeInt16(BlockAddress + 304, 15);
      builder = writeStoredEntry(builder, 0, 12, 10, 500);
      builder = writeStoredEntry(builder, 1, 15, 25, 1200);
      builder = writeStoredEntry(builder, 2, 99, 5, 0);

      const resource = readSynthetic(builder, statusAddresses);

      expect(resource.digievolutions).toEqual([
        { index: 1, digievolutionId: 12 },
        { index: 2, digievolutionId: 15 },
      ]);
      expect(resource.storedDigievolutions).toEqual([
        { digievolutionId: 12, level: 10, dvexp: 500 },
        { digievolutionId: 15, level: 25, dvexp: 1200 },
        { digievolutionId: 99, level: 5, dvexp: 0 },
      ]);
    });

    it.each([
      [85, 85],
      [0, 0],
    ])("should read a digievolution slot id %i as %i", (rawId, expectedId) => {
      const statusAddresses: DigimonStatusAddresses = {
        ...SyntheticStatusAddresses,
        digievolutions: {
          ...SyntheticStatusAddresses.digievolutions,
          slots: [{ index: 3, address: 64 }],
        },
      };

      const resource = readSynthetic(
        new FakeSnapshotBuilder().writeInt16(BlockAddress + 64, rawId),
        statusAddresses,
      );

      expect(resource.digievolutions).toEqual([{ index: 3, digievolutionId: expectedId }]);
    });

    it("should read the in battle data of the given party slot", () => {
      const builder = new FakeSnapshotBuilder().writeInt16(0x000a4470 + 0x20 + 0x10, 252);

      const resource = readSynthetic(builder, SyntheticStatusAddresses, 2);

      expect(resource.inBattle.strength).toBe(252);
    });

    it("should throw MemoryReadError when the block is outside the snapshot", () => {
      expect(() =>
        DigimonReader.read(
          new MemorySnapshot(new FakeSnapshotBuilder().build()),
          { ...SyntheticDigimonAddress, memoryBlockAddress: PlayStationRamSize },
          SyntheticStatusAddresses,
          InBattleAddresses,
          1,
        ),
      ).toThrow(MemoryReadError);
    });
  });

  describe("read stored digievolutions", () => {
    it("should return every unlocked entry when the ids are greater than zero", () => {
      let builder = writeStoredEntry(new FakeSnapshotBuilder(), 0, 3, 15, 70000);
      builder = writeStoredEntry(builder, 1, 5, 40, 250);
      builder = writeStoredEntry(builder, 2, 8, 20, 0);

      const resource = readSynthetic(builder);

      expect(resource.storedDigievolutions).toEqual([
        { digievolutionId: 3, level: 15, dvexp: 70000 },
        { digievolutionId: 5, level: 40, dvexp: 250 },
        { digievolutionId: 8, level: 20, dvexp: 0 },
      ]);
    });

    it("should return an empty list when the first entry is empty", () => {
      let builder = writeStoredEntry(new FakeSnapshotBuilder(), 0, 0, 15, 100);
      builder = writeStoredEntry(builder, 1, 99, 40, 100);

      const resource = readSynthetic(builder);

      expect(resource.storedDigievolutions).toEqual([]);
    });

    it("should stop at the first empty entry and ignore the garbage after it", () => {
      let builder = writeStoredEntry(new FakeSnapshotBuilder(), 0, 3, 15, 100);
      builder = writeStoredEntry(builder, 1, 0, 0, 0);
      builder = writeStoredEntry(builder, 2, 240, 180, 999);

      const resource = readSynthetic(builder);

      expect(resource.storedDigievolutions).toEqual([
        { digievolutionId: 3, level: 15, dvexp: 100 },
      ]);
    });

    it("should stop at a negative id", () => {
      let builder = writeStoredEntry(new FakeSnapshotBuilder(), 0, 3, 15, 100);
      builder = writeStoredEntry(builder, 1, -1, 1, 1);

      const resource = readSynthetic(builder);

      expect(resource.storedDigievolutions).toHaveLength(1);
    });

    it("should respect the maximum of unlocked digievolutions", () => {
      const statusAddresses: DigimonStatusAddresses = {
        ...SyntheticStatusAddresses,
        digievolutions: {
          ...SyntheticStatusAddresses.digievolutions,
          maxUnlockedDigievolutions: 2,
        },
      };
      let builder = writeStoredEntry(new FakeSnapshotBuilder(), 0, 3, 15, 100);
      builder = writeStoredEntry(builder, 1, 5, 40, 200);
      builder = writeStoredEntry(builder, 2, 8, 20, 300);

      const resource = readSynthetic(builder, statusAddresses);

      expect(resource.storedDigievolutions.map((stored) => stored.digievolutionId)).toEqual([3, 5]);
    });

    it("should read negative dvexp as signed", () => {
      const builder = writeStoredEntry(new FakeSnapshotBuilder(), 0, 3, 15, -1);

      const resource = readSynthetic(builder);

      expect(resource.storedDigievolutions[0]?.dvexp).toBe(-1);
    });
  });

  describe("read with the real definitions", () => {
    it("should read Kumamon from its block", () => {
      const block = 0x00049878;
      const builder = new FakeSnapshotBuilder()
        .writeInt32(block + 0x18, 85000)
        .writeInt16(block + 0x1c, 45)
        .writeInt16(block + 0x20, 500)
        .writeInt16(block + 0x22, 1000)
        .writeInt16(block + 0x24, 200)
        .writeInt16(block + 0x26, 400)
        .writeInt16(block + 0x28, 80)
        .writeInt16(block + 0x2a, 75)
        .writeInt16(block + 0x2c, 60)
        .writeInt16(block + 0x2e, 95)
        .writeInt16(block + 0x30, 110)
        .writeInt16(block + 0x32, 40)
        .writeInt16(block + 0x34, 10)
        .writeInt16(block + 0x36, 20)
        .writeInt16(block + 0x38, 30)
        .writeInt16(block + 0x3a, 40)
        .writeInt16(block + 0x3c, 50)
        .writeInt16(block + 0x3e, 60)
        .writeInt16(block + 0x40, 70)
        .writeInt16(block + 0x3c0, 101)
        .writeInt16(block + 0x3c2, 102)
        .writeInt16(block + 0x3c4, 103)
        .writeInt16(block + 0x3c6, 104)
        .writeInt16(block + 0x3c8, 105)
        .writeInt16(block + 0x3ca, 106)
        .writeInt16(block + 0x48, 5)
        .writeInt16(block + 0x4a, 10)
        .writeInt16(block + 0x4c, 0)
        .writeInt16(block + 0x50, 5)
        .writeInt16(block + 0x52, 3)
        .writeInt16(block - 4, 5)
        .writeInt16(0x00042b76, 400);

      const resource = readReal(builder, 1);

      expect(resource).toMatchObject({
        activeDigievolutionId: 5,
        experience: 85000,
        level: 45,
        blast: 400,
        hp: { current: 500, max: 1000 },
        mp: { current: 200, max: 400 },
        attributes: { strength: 80, defense: 75, spirit: 60, wisdom: 95, speed: 110, charisma: 40 },
        elements: { fire: 10, water: 20, ice: 30, wind: 40, thunder: 50, machine: 60, dark: 70 },
        equipments: {
          head: 101,
          body: 102,
          right: 103,
          left: 104,
          accessory1: 105,
          accessory2: 106,
        },
        digievolutions: [
          { index: 1, digievolutionId: 5 },
          { index: 2, digievolutionId: 10 },
          { index: 3, digievolutionId: 0 },
        ],
        storedDigievolutions: [{ digievolutionId: 5, level: 3, dvexp: 0 }],
      });
    });

    it("should read Patamon from another block", () => {
      const block = 0x0004afa0;
      const builder = new FakeSnapshotBuilder()
        .writeInt32(block + 0x18, 12000)
        .writeInt16(block + 0x1c, 18)
        .writeInt16(block - 4, 7)
        .writeInt16(0x00042b82, 999);

      const resource = readReal(builder, 7);

      expect(resource).toMatchObject({
        activeDigievolutionId: 7,
        experience: 12000,
        level: 18,
        blast: 999,
      });
    });

    it("should read the active digievolution four bytes before the block", () => {
      const block = 0x0004949c;
      const builder = new FakeSnapshotBuilder().writeInt16(block - 4, 389);

      const resource = readReal(builder, 0);

      expect(resource.activeDigievolutionId).toBe(389);
    });
  });
});
