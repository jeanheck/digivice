import { describe, expect, it } from "vitest";
import { DigimonAssembler } from "@/memory/assemblers/digimon.assembler";
import type { DigimonResource } from "@/memory/resources/parties/digimon.resource";

function digimonResource(overrides: Partial<DigimonResource> = {}): DigimonResource {
  return {
    activeDigievolutionId: 0,
    experience: 100,
    level: 1,
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
    ...overrides,
  };
}

describe("DigimonAssembler", () => {
  describe("assemble", () => {
    it.each([
      [0, null],
      [-1, null],
      [5, 5],
    ])("should map the active digievolution id %i to %s", (rawId, expectedId) => {
      const digimon = DigimonAssembler.assemble(digimonResource({ activeDigievolutionId: rawId }));

      expect(digimon.activeDigievolutionId).toBe(expectedId);
    });

    it.each([
      [0, null],
      [-1, null],
      [101, 101],
    ])("should map every equipment id %i to %s", (rawId, expectedId) => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({
          equipments: {
            head: rawId,
            body: rawId,
            right: rawId,
            left: rawId,
            accessory1: rawId,
            accessory2: rawId,
          },
        }),
      );

      expect(digimon.equipments).toEqual({
        head: expectedId,
        body: expectedId,
        right: expectedId,
        left: expectedId,
        accessory1: expectedId,
        accessory2: expectedId,
      });
    });

    it("should map every sub structure", () => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({
          experience: 5000,
          level: 30,
          tp: 15,
          blast: 600,
          activeDigievolutionId: 12,
          hp: { current: 100, max: 120 },
          mp: { current: 50, max: 60 },
          inBattle: {
            condition: 0x04,
            strength: 25,
            defense: 185,
            speed: 84,
            hp: { current: 80, max: 120 },
            mp: { current: 40, max: 60 },
          },
          attributes: {
            strength: 10,
            defense: 11,
            spirit: 12,
            wisdom: 13,
            speed: 14,
            charisma: 15,
          },
          elements: { fire: 1, water: 2, ice: 3, wind: 4, thunder: 5, machine: 6, dark: 7 },
          equipments: {
            head: 101,
            body: 102,
            right: 103,
            left: 104,
            accessory1: 105,
            accessory2: 106,
          },
          digievolutions: [{ index: 1, digievolutionId: 1 }],
          storedDigievolutions: [
            { digievolutionId: 1, level: 5, dvexp: 200 },
            { digievolutionId: 99, level: 12, dvexp: 0 },
          ],
        }),
      );

      expect(digimon).toEqual({
        level: 30,
        tp: 15,
        blast: 600,
        experience: 5000,
        hp: { current: 100, max: 120 },
        mp: { current: 50, max: 60 },
        inBattle: {
          condition: 0x04,
          strength: 25,
          defense: 185,
          speed: 84,
          hp: { current: 80, max: 120 },
          mp: { current: 40, max: 60 },
        },
        attributes: { strength: 10, defense: 11, spirit: 12, wisdom: 13, speed: 14, charisma: 15 },
        elements: { fire: 1, water: 2, ice: 3, wind: 4, thunder: 5, machine: 6, dark: 7 },
        equipments: {
          head: 101,
          body: 102,
          right: 103,
          left: 104,
          accessory1: 105,
          accessory2: 106,
        },
        digievolutions: [{ index: 1, digievolutionId: 1, digievolution: { level: 5, dvexp: 200 } }],
        storedDigievolutions: [
          { digievolutionId: 1, level: 5 },
          { digievolutionId: 99, level: 12 },
        ],
        activeDigievolutionId: 12,
      });
    });

    it("should not share nested objects with the resource", () => {
      const resource = digimonResource();

      const digimon = DigimonAssembler.assemble(resource);

      expect(digimon.hp).not.toBe(resource.hp);
      expect(digimon.inBattle).not.toBe(resource.inBattle);
      expect(digimon.attributes).not.toBe(resource.attributes);
      expect(digimon.elements).not.toBe(resource.elements);
    });
  });

  describe("assemble digievolution slots", () => {
    it("should use the stored digievolution when its id is found", () => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({
          digievolutions: [{ index: 1, digievolutionId: 32 }],
          storedDigievolutions: [
            { digievolutionId: 10, level: 3, dvexp: 50 },
            { digievolutionId: 32, level: 15, dvexp: 700 },
          ],
        }),
      );

      expect(digimon.digievolutions).toEqual([
        { index: 1, digievolutionId: 32, digievolution: { level: 15, dvexp: 700 } },
      ]);
    });

    it("should use the default level and dvexp when the id is not stored", () => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({
          digievolutions: [{ index: 2, digievolutionId: 32 }],
          storedDigievolutions: [{ digievolutionId: 10, level: 3, dvexp: 50 }],
        }),
      );

      expect(digimon.digievolutions).toEqual([
        { index: 2, digievolutionId: 32, digievolution: { level: 1, dvexp: 0 } },
      ]);
    });

    it.each([
      [-1, 0],
      [-0x80000000, 0],
      [0, 0],
      [1, 1],
    ])("should map a stored dvexp %i to %i", (rawDvexp, expectedDvexp) => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({
          digievolutions: [{ index: 1, digievolutionId: 32 }],
          storedDigievolutions: [{ digievolutionId: 32, level: 4, dvexp: rawDvexp }],
        }),
      );

      expect(digimon.digievolutions[0]?.digievolution?.dvexp).toBe(expectedDvexp);
    });

    it.each([0, -1])("should return an empty slot when the digievolution id is %i", (rawId) => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({ digievolutions: [{ index: 3, digievolutionId: rawId }] }),
      );

      expect(digimon.digievolutions).toEqual([
        { index: 3, digievolutionId: null, digievolution: null },
      ]);
    });
  });

  describe("assemble stored digievolutions", () => {
    it("should keep the id and the level and drop the dvexp", () => {
      const digimon = DigimonAssembler.assemble(
        digimonResource({ storedDigievolutions: [{ digievolutionId: 7, level: 22, dvexp: 300 }] }),
      );

      expect(digimon.storedDigievolutions).toEqual([{ digievolutionId: 7, level: 22 }]);
    });
  });
});
