import { describe, expect, it } from "vitest";
import { AuctionsAssembler } from "@/memory/assemblers/auctions.assembler";

describe("AuctionsAssembler", () => {
  describe("assemble", () => {
    it("should map non-zero flags to true and zero flags to false", () => {
      const auctions = AuctionsAssembler.assemble({
        divineBarrier: 0x01,
        hazardShield: 0x00,
        sniperShield: 0x04,
        dramonShield: 0x00,
        yinYangWand: 0x10,
        hazardFang: 0x20,
        sniperCannon: 0x00,
        roninBlade: 0x80,
        sniperGoggle: 0x01,
        divineCrown: 0x00,
        braveHelmet: 0x04,
      });

      expect(auctions).toEqual({
        divineBarrier: true,
        hazardShield: false,
        sniperShield: true,
        dramonShield: false,
        yinYangWand: true,
        hazardFang: true,
        sniperCannon: false,
        roninBlade: true,
        sniperGoggle: true,
        divineCrown: false,
        braveHelmet: true,
      });
    });

    it("should map every flag to false when all values are zero", () => {
      const auctions = AuctionsAssembler.assemble({
        divineBarrier: 0,
        hazardShield: 0,
        sniperShield: 0,
        dramonShield: 0,
        yinYangWand: 0,
        hazardFang: 0,
        sniperCannon: 0,
        roninBlade: 0,
        sniperGoggle: 0,
        divineCrown: 0,
        braveHelmet: 0,
      });

      expect(Object.values(auctions).every((consumed) => consumed === false)).toBe(true);
    });
  });
});
