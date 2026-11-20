import { describe, expect, it } from "vitest";
import type { EnemyAddresses } from "@/memory/addresses/enemy.addresses";
import { DigimonBattleReader } from "@/memory/readers/digimon-battle.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const SyntheticEnemyAddresses: EnemyAddresses = {
  enemySlotBase: 0x1000,
  slotStride: 0x20,
  slotCount: 3,
  activeEnemySlotIndex: 0x2000,
  groupId: 0x3000,
  id: 0x00,
  condition: 0x1c,
  strength: 0x10,
  defense: 0x12,
  speed: 0x14,
  hp: { max: 0x06, current: 0x08 },
};

describe("DigimonBattleReader", () => {
  describe("read", () => {
    it("should map the field and the enemy when the addresses are synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x4000, 3)
          .writeInt16(0x1000, 122)
          .writeInt16(0x1006, 672)
          .writeInt16(0x1008, 600)
          .writeInt16(0x1010, 70)
          .writeInt16(0x1012, 65)
          .writeInt16(0x1014, 84)
          .writeByte(0x101c, 0x01)
          .writeInt16(0x3000, 201)
          .build(),
      );

      const resource = DigimonBattleReader.read(
        snapshot,
        { field: 0x4000 },
        SyntheticEnemyAddresses,
      );

      expect(resource).toEqual({
        field: 3,
        enemy: {
          id: 122,
          groupId: 201,
          condition: 0x01,
          strength: 70,
          defense: 65,
          speed: 84,
          hp: { current: 600, max: 672 },
        },
      });
    });

    it("should map the field and the enemy when the addresses come from the real definitions", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x000a4530, 2)
          .writeInt16(0x000a446c, 1)
          .writeInt16(0x000a44f0, 25)
          .writeInt16(0x000a44f6, 192)
          .writeInt16(0x000a44f8, 150)
          .writeInt16(0x00042b2c, 201)
          .build(),
      );

      const resource = DigimonBattleReader.read(
        snapshot,
        AddressesRepository.getDigimonBattleAddresses(),
        AddressesRepository.getEnemyAddresses(),
      );

      expect(resource.field).toBe(2);
      expect(resource.enemy.id).toBe(25);
      expect(resource.enemy.groupId).toBe(201);
      expect(resource.enemy.hp).toEqual({ current: 150, max: 192 });
    });

    it("should throw MemoryReadError when the field address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() =>
        DigimonBattleReader.read(snapshot, { field: PlayStationRamSize }, SyntheticEnemyAddresses),
      ).toThrow(MemoryReadError);
    });
  });
});
