import { describe, expect, it } from "vitest";
import type { PlayerAddresses } from "@/memory/addresses/player.addresses";
import { PlayerReader } from "@/memory/readers/player.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const SyntheticAddresses: PlayerAddresses = {
  bits: 0x1000,
  mapId: 0x3000,
  previousMapId: 0x3500,
  seabedRoute: 0x4000,
  mapVariant: 0x5000,
};

describe("PlayerReader", () => {
  describe("read", () => {
    it("should map the player resource when the addresses are synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeInt32(0x1000, 15000)
          .writeInt16(0x3000, 4)
          .writeInt16(0x3500, 0x023e)
          .writeByte(0x4000, 0x08)
          .writeByte(0x5000, 0x01)
          .build(),
      );

      const resource = PlayerReader.read(snapshot, SyntheticAddresses);

      expect(resource).toEqual({
        bits: 15000,
        mapId: 4,
        previousMapId: 0x023e,
        seabedRoute: 0x08,
        mapVariant: 0x01,
      });
    });

    it("should map the player resource when the addresses come from the real definitions", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeInt32(0x00048da0, 15000)
          .writeInt16(0x0004b3f8, 4)
          .writeInt16(0x0004b400, 0x023e)
          .writeByte(0x00048d78, 0x08)
          .writeByte(0x00048d7a, 0x01)
          .build(),
      );

      const resource = PlayerReader.read(snapshot, AddressesRepository.getPlayerAddresses());

      expect(resource).toEqual({
        bits: 15000,
        mapId: 4,
        previousMapId: 0x023e,
        seabedRoute: 0x08,
        mapVariant: 0x01,
      });
    });

    it("should read map ids as signed when the RAM holds 0xFFFF", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt16(0x3000, 0xffff).writeInt16(0x3500, -1).build(),
      );

      const resource = PlayerReader.read(snapshot, SyntheticAddresses);

      expect(resource.mapId).toBe(-1);
      expect(resource.previousMapId).toBe(-1);
    });

    it("should read zeros when the RAM is empty", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      const resource = PlayerReader.read(snapshot, SyntheticAddresses);

      expect(resource).toEqual({
        bits: 0,
        mapId: 0,
        previousMapId: 0,
        seabedRoute: 0,
        mapVariant: 0,
      });
    });

    it("should throw MemoryReadError when an address is outside the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() =>
        PlayerReader.read(snapshot, { ...SyntheticAddresses, bits: PlayStationRamSize }),
      ).toThrow(MemoryReadError);
    });
  });
});
