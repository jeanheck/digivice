import { describe, expect, it } from "vitest";
import { ImportantItemsAssembler } from "@/memory/assemblers/important-items.assembler";

describe("ImportantItemsAssembler", () => {
  describe("assemble", () => {
    it("should map non-zero bytes to true", () => {
      const importantItems = ImportantItemsAssembler.assemble({
        treeBoots: 0x01,
        fishingPole: 0x02,
        asukaTrophy: 0xff,
        sunTrophy: 0x01,
      });

      expect(importantItems).toEqual({
        treeBoots: true,
        fishingPole: true,
        asukaTrophy: true,
        sunTrophy: true,
      });
    });

    it("should map zero to false", () => {
      const importantItems = ImportantItemsAssembler.assemble({
        treeBoots: 0,
        fishingPole: 0,
        asukaTrophy: 0,
        sunTrophy: 0,
      });

      expect(importantItems).toEqual({
        treeBoots: false,
        fishingPole: false,
        asukaTrophy: false,
        sunTrophy: false,
      });
    });

    it("should map each item independently when the values are mixed", () => {
      const importantItems = ImportantItemsAssembler.assemble({
        treeBoots: 0,
        fishingPole: 1,
        asukaTrophy: 0,
        sunTrophy: 0x80,
      });

      expect(importantItems).toEqual({
        treeBoots: false,
        fishingPole: true,
        asukaTrophy: false,
        sunTrophy: true,
      });
    });
  });
});
