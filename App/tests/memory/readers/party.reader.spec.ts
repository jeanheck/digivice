import { describe, expect, it } from "vitest";
import type { PartyAddresses } from "@/memory/addresses/party.addresses";
import { PartyReader } from "@/memory/readers/party.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { PartyResource } from "@/memory/resources/party.resource";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const EmptySlotId = 0xff;
const SlotAddresses = [0x00048da4, 0x00048da8, 0x00048dac];
const KumamonBlock = 0x00049878;

function withSlots(builder: FakeSnapshotBuilder, ...digimonIds: number[]): FakeSnapshotBuilder {
  digimonIds.forEach((digimonId, index) => {
    const slotAddress = SlotAddresses[index];
    if (slotAddress === undefined) {
      throw new Error(`Slot ${index + 1} does not exist.`);
    }
    builder.writeByte(slotAddress, digimonId);
  });
  return builder;
}

function readParty(
  builder: FakeSnapshotBuilder,
  partyAddresses: PartyAddresses = AddressesRepository.getPartyAddresses(),
): PartyResource {
  return PartyReader.read(
    new MemorySnapshot(builder.build()),
    partyAddresses,
    AddressesRepository.getDigimonsAddresses(),
    AddressesRepository.getDigimonStatusAddresses(),
    AddressesRepository.getInBattleAddresses(),
  );
}

describe("PartyReader", () => {
  describe("read", () => {
    it("should keep the slot index and the raw digimon id when the addresses are synthetic", () => {
      const partyAddresses: PartyAddresses = {
        emptySlotId: EmptySlotId,
        slots: [
          { index: 2, address: 0x5000 },
          { index: 3, address: 0x6000 },
        ],
      };
      const builder = new FakeSnapshotBuilder().writeByte(0x5000, 14).writeByte(0x6000, 0xff);

      const resource = readParty(builder, partyAddresses);

      expect(resource.slots).toEqual([
        { index: 2, digimonId: 14, digimon: null },
        { index: 3, digimonId: 0xff, digimon: null },
      ]);
    });

    it("should read the occupied slot and skip the empty ones", () => {
      const builder = withSlots(new FakeSnapshotBuilder(), 1, EmptySlotId, EmptySlotId)
        .writeInt32(KumamonBlock + 0x18, 1500)
        .writeInt16(KumamonBlock + 0x1c, 12)
        .writeInt16(KumamonBlock + 0x20, 450)
        .writeInt16(KumamonBlock + 0x22, 500)
        .writeInt16(KumamonBlock + 0x28, 42)
        .writeInt16(KumamonBlock + 0x48, 5)
        .writeInt16(KumamonBlock + 0x4a, 10)
        .writeInt16(KumamonBlock + 0x50, 5)
        .writeInt16(KumamonBlock + 0x52, 3)
        .writeInt16(KumamonBlock - 4, 5);

      const resource = readParty(builder);

      expect(resource.slots).toHaveLength(3);
      expect(resource.slots[0]).toMatchObject({
        index: 1,
        digimonId: 1,
        digimon: {
          activeDigievolutionId: 5,
          experience: 1500,
          level: 12,
          hp: { current: 450, max: 500 },
          attributes: { strength: 42 },
          digievolutions: [
            { index: 1, digievolutionId: 5 },
            { index: 2, digievolutionId: 10 },
            { index: 3, digievolutionId: 0 },
          ],
          storedDigievolutions: [{ digievolutionId: 5, level: 3, dvexp: 0 }],
        },
      });
      expect(resource.slots[1]).toEqual({ index: 2, digimonId: EmptySlotId, digimon: null });
      expect(resource.slots[2]).toEqual({ index: 3, digimonId: EmptySlotId, digimon: null });
    });

    it("should load only the occupied middle slot", () => {
      const builder = withSlots(new FakeSnapshotBuilder(), EmptySlotId, 2, EmptySlotId).writeInt16(
        0x00049c54 + 0x1c,
        8,
      );

      const resource = readParty(builder);

      expect(resource.slots[0]?.digimon).toBeNull();
      expect(resource.slots[1]?.digimonId).toBe(2);
      expect(resource.slots[1]?.digimon?.level).toBe(8);
      expect(resource.slots[2]?.digimon).toBeNull();
    });

    it("should not load a Digimon when the slot contains an unknown digimon id", () => {
      const resource = readParty(
        withSlots(new FakeSnapshotBuilder(), 99, EmptySlotId, EmptySlotId),
      );

      expect(resource.slots[0]).toEqual({ index: 1, digimonId: 99, digimon: null });
    });

    it("should not load a Digimon when a later slot contains an unknown digimon id", () => {
      const builder = withSlots(new FakeSnapshotBuilder(), 1, 99, EmptySlotId).writeInt16(
        KumamonBlock + 0x1c,
        12,
      );

      const resource = readParty(builder);

      expect(resource.slots[0]?.digimon?.level).toBe(12);
      expect(resource.slots[1]).toEqual({ index: 2, digimonId: 99, digimon: null });
    });

    it("should load Kotemon when the slot contains the digimon id zero", () => {
      const builder = withSlots(new FakeSnapshotBuilder(), 0, EmptySlotId, EmptySlotId)
        .writeInt16(0x0004949c + 0x1c, 7)
        .writeInt16(0x00042b74, 420);

      const resource = readParty(builder);

      expect(resource.slots[0]?.digimonId).toBe(0);
      expect(resource.slots[0]?.digimon?.level).toBe(7);
      expect(resource.slots[0]?.digimon?.blast).toBe(420);
    });

    it("should return every slot empty when all slots hold the empty id", () => {
      const resource = readParty(
        withSlots(new FakeSnapshotBuilder(), EmptySlotId, EmptySlotId, EmptySlotId),
      );

      expect(resource.slots).toEqual([
        { index: 1, digimonId: EmptySlotId, digimon: null },
        { index: 2, digimonId: EmptySlotId, digimon: null },
        { index: 3, digimonId: EmptySlotId, digimon: null },
      ]);
    });

    it("should keep the persistent vitals and populate the in battle data", () => {
      const allySlot = 0x000a4470;
      const builder = withSlots(new FakeSnapshotBuilder(), 1, EmptySlotId, EmptySlotId)
        .writeInt16(KumamonBlock + 0x20, 450)
        .writeInt16(KumamonBlock + 0x22, 500)
        .writeInt16(KumamonBlock + 0x24, 200)
        .writeInt16(KumamonBlock + 0x26, 300)
        .writeInt16(allySlot + 0x06, 1850)
        .writeInt16(allySlot + 0x08, 1400)
        .writeInt16(allySlot + 0x0a, 1140)
        .writeInt16(allySlot + 0x0c, 900)
        .writeInt16(allySlot + 0x10, 252)
        .writeInt16(allySlot + 0x12, 185)
        .writeInt16(allySlot + 0x14, 84)
        .writeByte(allySlot + 0x1c, 0x04);

      const digimon = readParty(builder).slots[0]?.digimon;

      expect(digimon?.hp).toEqual({ current: 450, max: 500 });
      expect(digimon?.mp).toEqual({ current: 200, max: 300 });
      expect(digimon?.inBattle).toEqual({
        condition: 0x04,
        strength: 252,
        defense: 185,
        speed: 84,
        hp: { current: 1400, max: 1850 },
        mp: { current: 900, max: 1140 },
      });
    });

    it("should read the in battle data from the ally slot of the party slot", () => {
      const builder = withSlots(new FakeSnapshotBuilder(), EmptySlotId, EmptySlotId, 1).writeInt16(
        0x000a4470 + 0x40 + 0x10,
        77,
      );

      const resource = readParty(builder);

      expect(resource.slots[2]?.digimon?.inBattle.strength).toBe(77);
    });

    it("should throw MemoryReadError when a slot address is outside the snapshot", () => {
      const partyAddresses: PartyAddresses = {
        emptySlotId: EmptySlotId,
        slots: [{ index: 1, address: PlayStationRamSize }],
      };

      expect(() => readParty(new FakeSnapshotBuilder(), partyAddresses)).toThrow(MemoryReadError);
    });
  });
});
