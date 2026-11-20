import { describe, expect, it } from "vitest";
import type { EnemyAddresses } from "@/memory/addresses/enemy.addresses";
import { EnemyReader } from "@/memory/readers/enemy.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

const SyntheticAddresses: EnemyAddresses = {
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

interface EnemySlotValues {
  id: number;
  currentHp: number;
  maxHp?: number;
  condition?: number;
  strength?: number;
  defense?: number;
  speed?: number;
}

function writeEnemySlot(
  builder: FakeSnapshotBuilder,
  slotIndex: number,
  values: EnemySlotValues,
): FakeSnapshotBuilder {
  const slotBase = SyntheticAddresses.enemySlotBase + slotIndex * SyntheticAddresses.slotStride;
  return builder
    .writeInt16(slotBase + SyntheticAddresses.id, values.id)
    .writeInt16(slotBase + SyntheticAddresses.hp.max, values.maxHp ?? values.currentHp)
    .writeInt16(slotBase + SyntheticAddresses.hp.current, values.currentHp)
    .writeInt16(slotBase + SyntheticAddresses.strength, values.strength ?? 0)
    .writeInt16(slotBase + SyntheticAddresses.defense, values.defense ?? 0)
    .writeInt16(slotBase + SyntheticAddresses.speed, values.speed ?? 0)
    .writeByte(slotBase + SyntheticAddresses.condition, values.condition ?? 0);
}

function readEnemy(builder: FakeSnapshotBuilder, activeEnemySlotIndex: number) {
  const snapshot = new MemorySnapshot(
    builder.writeInt16(SyntheticAddresses.activeEnemySlotIndex, activeEnemySlotIndex).build(),
  );
  return EnemyReader.read(snapshot, SyntheticAddresses);
}

describe("EnemyReader", () => {
  describe("read", () => {
    it("should map every field of the active slot when the addresses are synthetic", () => {
      const builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 122,
        currentHp: 600,
        maxHp: 672,
        condition: 0x01,
        strength: 70,
        defense: 65,
        speed: 84,
      }).writeInt16(SyntheticAddresses.groupId, 201);

      const resource = readEnemy(builder, 0);

      expect(resource).toEqual({
        id: 122,
        groupId: 201,
        condition: 0x01,
        strength: 70,
        defense: 65,
        speed: 84,
        hp: { current: 600, max: 672 },
      });
    });

    it("should read slot zero when only the first slot has a live enemy", () => {
      const builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, { id: 122, currentHp: 600 });

      const resource = readEnemy(builder, -1);

      expect(resource.id).toBe(122);
      expect(resource.hp.current).toBe(600);
    });

    it("should read the second slot when the first slot is KO and the active index is invalid", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 0,
        maxHp: 500,
      });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 400, maxHp: 800 });

      const resource = readEnemy(builder, -1);

      expect(resource.id).toBe(200);
      expect(resource.hp).toEqual({ current: 400, max: 800 });
    });

    it("should prefer the active slot index when multiple live enemy slots exist", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, { id: 100, currentHp: 500 });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 400 });

      const resource = readEnemy(builder, 1);

      expect(resource.id).toBe(200);
      expect(resource.hp.current).toBe(400);
    });

    it("should read the last occupied slot when all enemy slots are KO", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 0,
        maxHp: 500,
      });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 0, maxHp: 800 });

      const resource = readEnemy(builder, -1);

      expect(resource.id).toBe(200);
      expect(resource.hp.current).toBe(0);
    });

    it("should stay on a KO enemy when the active slot index still points to it", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 0,
        maxHp: 500,
      });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 400 });

      const resource = readEnemy(builder, 0);

      expect(resource.id).toBe(100);
      expect(resource.hp.current).toBe(0);
    });

    it("should read the first live slot when the active slot index points to an empty slot", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 0,
        maxHp: 500,
      });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 400 });

      const resource = readEnemy(builder, 2);

      expect(resource.id).toBe(200);
      expect(resource.hp.current).toBe(400);
    });

    it("should read the first live slot when the active slot index is out of range", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 0,
        maxHp: 500,
      });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 400 });

      const resource = readEnemy(builder, 3);

      expect(resource.id).toBe(200);
      expect(resource.hp.current).toBe(400);
    });

    it("should read the third slot when all three enemies are KO", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 0,
        maxHp: 500,
      });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 0, maxHp: 800 });
      builder = writeEnemySlot(builder, 2, { id: 300, currentHp: 0, maxHp: 900 });

      const resource = readEnemy(builder, -1);

      expect(resource.id).toBe(300);
      expect(resource.hp.current).toBe(0);
    });

    it("should read the third slot when the active slot index points to it and prior enemies are alive", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, { id: 100, currentHp: 500 });
      builder = writeEnemySlot(builder, 1, { id: 200, currentHp: 400 });
      builder = writeEnemySlot(builder, 2, { id: 110, currentHp: 552 });

      const resource = readEnemy(builder, 2);

      expect(resource.id).toBe(110);
      expect(resource.hp).toEqual({ current: 552, max: 552 });
    });

    it("should return an enemy with id zero when all enemy slots are empty", () => {
      const resource = readEnemy(new FakeSnapshotBuilder(), -1);

      expect(resource).toEqual({
        id: 0,
        groupId: 0,
        condition: 0,
        strength: 0,
        defense: 0,
        speed: 0,
        hp: { current: 0, max: 0 },
      });
    });

    it("should read the group id from its global address regardless of the active slot", () => {
      let builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, { id: 100, currentHp: 500 });
      builder = writeEnemySlot(builder, 2, { id: 300, currentHp: 900 });
      builder.writeInt16(SyntheticAddresses.groupId, 201);

      const resource = readEnemy(builder, 2);

      expect(resource.id).toBe(300);
      expect(resource.groupId).toBe(201);
    });

    it("should read signed values when the RAM holds 0xFFFF", () => {
      const builder = writeEnemySlot(new FakeSnapshotBuilder(), 0, {
        id: 100,
        currentHp: 500,
        strength: 0xffff,
      }).writeInt16(SyntheticAddresses.groupId, 0xffff);

      const resource = readEnemy(builder, 0);

      expect(resource.strength).toBe(-1);
      expect(resource.groupId).toBe(-1);
    });

    it("should read the Vegiemon slot when the snapshot is the battle against Natsumi fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("battle-against-natsumi"));

      const resource = EnemyReader.read(snapshot, AddressesRepository.getEnemyAddresses());

      expect(resource).toEqual({
        id: 25,
        groupId: 201,
        condition: 0,
        strength: 0,
        defense: 0,
        speed: 0,
        hp: { current: 192, max: 192 },
      });
    });

    it("should throw MemoryReadError when an address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() =>
        EnemyReader.read(snapshot, { ...SyntheticAddresses, groupId: PlayStationRamSize }),
      ).toThrow(MemoryReadError);
    });
  });
});
