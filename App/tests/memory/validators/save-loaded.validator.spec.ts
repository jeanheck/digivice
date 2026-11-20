import { describe, expect, it } from "vitest";
import { SaveLoadedValidator } from "@/memory/validators/save-loaded.validator";
import type { Party } from "@/state/models";
import { BattleAgainstNatsumiExpected } from "../../fixtures/snapshots/battle-against-natsumi.expected";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";

function createCentralParkParty(): Party {
  return structuredClone(FieldCentralParkExpected.party);
}

function createPartyWithFirstSlotLevel(level: number): Party {
  const party = createCentralParkParty();
  party.slots[0]!.digimon!.level = level;
  return party;
}

describe("SaveLoadedValidator", () => {
  describe("findViolation", () => {
    it("should accept the party when it is the Central Park fixture", () => {
      expect(SaveLoadedValidator.findViolation(FieldCentralParkExpected.party)).toBeNull();
    });

    it("should accept the party when only one slot is occupied", () => {
      expect(SaveLoadedValidator.findViolation(BattleAgainstNatsumiExpected.party)).toBeNull();
    });

    it("should report a violation when no slot is occupied", () => {
      const party = createCentralParkParty();
      party.slots = party.slots.map((slot) => ({ ...slot, digimonId: null, digimon: null }));

      expect(SaveLoadedValidator.findViolation(party)).toBe("No occupied party slot.");
    });

    it("should report a violation when a digimonId is repeated", () => {
      const party = createCentralParkParty();
      party.slots[1]!.digimonId = party.slots[0]!.digimonId;

      expect(SaveLoadedValidator.findViolation(party)).toContain("Repeated digimonId");
    });

    it("should report a violation when an occupied slot has level 0", () => {
      expect(SaveLoadedValidator.findViolation(createPartyWithFirstSlotLevel(0))).toBe(
        "Party slot 1 has level 0.",
      );
    });

    it("should report a violation when an occupied slot has level 100", () => {
      expect(SaveLoadedValidator.findViolation(createPartyWithFirstSlotLevel(100))).toBe(
        "Party slot 1 has level 100.",
      );
    });

    it("should accept the party when the levels are at the limits 1 and 99", () => {
      const party = createPartyWithFirstSlotLevel(1);
      party.slots[1]!.digimon!.level = 99;

      expect(SaveLoadedValidator.findViolation(party)).toBeNull();
    });
  });
});
