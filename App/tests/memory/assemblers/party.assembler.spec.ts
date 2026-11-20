import { describe, expect, it } from "vitest";
import { PartyAssembler } from "@/memory/assemblers/party.assembler";
import type { DigimonResource } from "@/memory/resources/parties/digimon.resource";

const EmptySlotId = 0xff;

function digimonResource(level: number): DigimonResource {
  return {
    activeDigievolutionId: 0,
    experience: 100,
    level,
    tp: 0,
    blast: 0,
    hp: { current: 0, max: 0 },
    mp: { current: 0, max: 0 },
    inBattle: {
      condition: 0,
      strength: 0,
      defense: 0,
      speed: 0,
      hp: { current: 0, max: 0 },
      mp: { current: 0, max: 0 },
    },
    attributes: { strength: 0, defense: 0, spirit: 0, wisdom: 0, speed: 0, charisma: 0 },
    elements: { fire: 0, water: 0, ice: 0, wind: 0, thunder: 0, machine: 0, dark: 0 },
    equipments: { head: 0, body: 0, right: 0, left: 0, accessory1: 0, accessory2: 0 },
    digievolutions: [],
    storedDigievolutions: [],
  };
}

describe("PartyAssembler", () => {
  describe("assemble", () => {
    it("should map occupied and empty slots", () => {
      const party = PartyAssembler.assemble({
        slots: [
          { index: 1, digimonId: 1, digimon: digimonResource(2) },
          { index: 2, digimonId: EmptySlotId, digimon: null },
          { index: 3, digimonId: EmptySlotId, digimon: null },
        ],
      });

      expect(party.slots).toHaveLength(3);
      expect(party.slots[0]).toMatchObject({ index: 1, digimonId: 1, digimon: { level: 2 } });
      expect(party.slots[1]).toEqual({ index: 2, digimonId: null, digimon: null });
      expect(party.slots[2]).toEqual({ index: 3, digimonId: null, digimon: null });
    });

    it("should keep Kotemon as an occupied slot when the digimon id is zero", () => {
      const party = PartyAssembler.assemble({
        slots: [{ index: 1, digimonId: 0, digimon: digimonResource(3) }],
      });

      expect(party.slots[0]).toMatchObject({ index: 1, digimonId: 0, digimon: { level: 3 } });
    });

    it.each([EmptySlotId, 42])(
      "should return an empty slot when the Digimon was not read for the digimon id %i",
      (rawDigimonId) => {
        const party = PartyAssembler.assemble({
          slots: [{ index: 2, digimonId: rawDigimonId, digimon: null }],
        });

        expect(party.slots).toEqual([{ index: 2, digimonId: null, digimon: null }]);
      },
    );
  });
});
