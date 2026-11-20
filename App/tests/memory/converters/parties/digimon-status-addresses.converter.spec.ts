import { describe, expect, it } from "vitest";
import type { DigimonStatusAddressesRaw } from "@/memory/addresses/parties/digimon-status.addresses";
import { DigimonStatusAddressesConverter } from "@/memory/converters/parties/digimon-status-addresses.converter";

function createRaw(): DigimonStatusAddressesRaw {
  return {
    experience: "0x18",
    level: "0x1C",
    tp: "0x1E",
    hp: { current: "0x20", max: "0x22" },
    mp: { current: "0x24", max: "0x26" },
    attributes: {
      strength: "0x28",
      defense: "0x2A",
      spirit: "0x2C",
      wisdom: "0x2E",
      speed: "0x30",
      charisma: "0x32",
    },
    elements: {
      fire: "0x34",
      water: "0x36",
      ice: "0x38",
      wind: "0x3A",
      thunder: "0x3C",
      machine: "0x3E",
      dark: "0x40",
    },
    equipments: {
      head: "0x3C0",
      body: "0x3C2",
      right: "0x3C4",
      left: "0x3C6",
      accessory1: "0x3C8",
      accessory2: "0x3CA",
    },
    digievolutions: {
      activeDigievolution: "-4",
      slots: [
        { index: 1, address: "0x48" },
        { index: 2, address: "0x4A" },
      ],
      unlockedDigievolutionsStart: "0x50",
      unlockedDigievolutionEntryStride: "20",
      maxUnlockedDigievolutions: "60",
      id: "0x00",
      level: "0x02",
      dvexp: "0x04",
    },
  };
}

describe("DigimonStatusAddressesConverter", () => {
  describe("convert", () => {
    it("should convert every offset of the Digimon status", () => {
      expect(DigimonStatusAddressesConverter.convert(createRaw())).toEqual({
        experience: 0x18,
        level: 0x1c,
        tp: 0x1e,
        hp: { current: 0x20, max: 0x22 },
        mp: { current: 0x24, max: 0x26 },
        attributes: {
          strength: 0x28,
          defense: 0x2a,
          spirit: 0x2c,
          wisdom: 0x2e,
          speed: 0x30,
          charisma: 0x32,
        },
        elements: {
          fire: 0x34,
          water: 0x36,
          ice: 0x38,
          wind: 0x3a,
          thunder: 0x3c,
          machine: 0x3e,
          dark: 0x40,
        },
        equipments: {
          head: 0x3c0,
          body: 0x3c2,
          right: 0x3c4,
          left: 0x3c6,
          accessory1: 0x3c8,
          accessory2: 0x3ca,
        },
        digievolutions: {
          activeDigievolution: -4,
          slots: [
            { index: 1, address: 0x48 },
            { index: 2, address: 0x4a },
          ],
          unlockedDigievolutionsStart: 0x50,
          unlockedDigievolutionEntryStride: 20,
          maxUnlockedDigievolutions: 60,
          id: 0,
          level: 2,
          dvexp: 4,
        },
      });
    });

    it("should throw when a scalar offset is missing", () => {
      const raw = createRaw();
      delete (raw.hp as Partial<typeof raw.hp>).max;

      expect(() => DigimonStatusAddressesConverter.convert(raw)).toThrow("Invalid address value");
    });
  });
});
