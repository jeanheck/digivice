import { describe, expect, it } from "vitest";
import type { AuctionsAddresses } from "@/memory/addresses/auctions.addresses";
import { AuctionsReader } from "@/memory/readers/auctions.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const FirstByteAddress = 0x0004b38a;
const SecondByteAddress = 0x0004b38b;

const SyntheticAddresses: AuctionsAddresses = {
  divineBarrier: { address: FirstByteAddress, bitMask: 0x01 },
  hazardShield: { address: FirstByteAddress, bitMask: 0x02 },
  sniperShield: { address: FirstByteAddress, bitMask: 0x04 },
  dramonShield: { address: FirstByteAddress, bitMask: 0x08 },
  yinYangWand: { address: FirstByteAddress, bitMask: 0x10 },
  hazardFang: { address: FirstByteAddress, bitMask: 0x20 },
  sniperCannon: { address: FirstByteAddress, bitMask: 0x40 },
  roninBlade: { address: FirstByteAddress, bitMask: 0x80 },
  sniperGoggle: { address: SecondByteAddress, bitMask: 0x01 },
  divineCrown: { address: SecondByteAddress, bitMask: 0x02 },
  braveHelmet: { address: SecondByteAddress, bitMask: 0x04 },
};

function createSnapshot(firstByteValue: number, secondByteValue: number): MemorySnapshot {
  return new MemorySnapshot(
    new FakeSnapshotBuilder()
      .writeByte(FirstByteAddress, firstByteValue)
      .writeByte(SecondByteAddress, secondByteValue)
      .build(),
  );
}

describe("AuctionsReader", () => {
  describe("read", () => {
    it("should map all auction flags from both bytes", () => {
      const resource = AuctionsReader.read(createSnapshot(0x65, 0x05), SyntheticAddresses);

      expect(resource).toEqual({
        divineBarrier: 0x01,
        hazardShield: 0x00,
        sniperShield: 0x04,
        dramonShield: 0x00,
        yinYangWand: 0x00,
        hazardFang: 0x20,
        sniperCannon: 0x40,
        roninBlade: 0x00,
        sniperGoggle: 0x01,
        divineCrown: 0x00,
        braveHelmet: 0x04,
      });
    });

    it("should return zero flags when both bytes are zero", () => {
      const resource = AuctionsReader.read(createSnapshot(0x00, 0x00), SyntheticAddresses);

      expect(Object.values(resource).every((value) => value === 0)).toBe(true);
    });

    it("should ignore bits outside the auction masks", () => {
      const resource = AuctionsReader.read(createSnapshot(0x00, 0xf8), SyntheticAddresses);

      expect(Object.values(resource).every((value) => value === 0)).toBe(true);
    });

    it("should map the auctions resource when the addresses come from the real definitions", () => {
      const resource = AuctionsReader.read(
        createSnapshot(0x83, 0x02),
        AddressesRepository.getAuctionsAddresses(),
      );

      expect(resource).toEqual({
        divineBarrier: 0x01,
        hazardShield: 0x02,
        sniperShield: 0x00,
        dramonShield: 0x00,
        yinYangWand: 0x00,
        hazardFang: 0x00,
        sniperCannon: 0x00,
        roninBlade: 0x80,
        sniperGoggle: 0x00,
        divineCrown: 0x02,
        braveHelmet: 0x00,
      });
    });

    it("should throw MemoryReadError when an address is outside the snapshot", () => {
      const addresses: AuctionsAddresses = {
        ...SyntheticAddresses,
        braveHelmet: { address: PlayStationRamSize, bitMask: 0x04 },
      };

      expect(() => AuctionsReader.read(createSnapshot(0x00, 0x00), addresses)).toThrow(
        MemoryReadError,
      );
    });
  });
});
