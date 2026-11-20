import { describe, expect, it } from "vitest";
import { PlayerAssembler } from "@/memory/assemblers/player.assembler";
import type { PlayerResource } from "@/memory/resources/player.resource";

function createResource(overrides: Partial<PlayerResource> = {}): PlayerResource {
  return {
    bits: 0,
    mapId: 0,
    previousMapId: 0,
    seabedRoute: 0,
    mapVariant: 0,
    ...overrides,
  };
}

describe("PlayerAssembler", () => {
  describe("assemble", () => {
    it("should map all fields when the resource is valid", () => {
      const player = PlayerAssembler.assemble(
        createResource({
          bits: 1500,
          mapId: 255,
          previousMapId: 0x023e,
          seabedRoute: 0x08,
          mapVariant: 0x01,
        }),
      );

      expect(player).toEqual({
        bits: 1500,
        mapId: "00FF",
        previousMapId: "023E",
        seabedRoute: 0x08,
        mapVariant: 0x01,
      });
    });

    it("should normalize seabedRoute and mapVariant to null when they are zero", () => {
      const player = PlayerAssembler.assemble(createResource());

      expect(player).toEqual({
        bits: 0,
        mapId: "0000",
        previousMapId: "0000",
        seabedRoute: null,
        mapVariant: null,
      });
    });

    it("should keep seabedRoute and mapVariant when they are one", () => {
      const player = PlayerAssembler.assemble(createResource({ seabedRoute: 1, mapVariant: 1 }));

      expect(player.seabedRoute).toBe(1);
      expect(player.mapVariant).toBe(1);
    });

    it("should format map ids as FFFF when the raw value is -1", () => {
      const player = PlayerAssembler.assemble(createResource({ mapId: -1, previousMapId: -1 }));

      expect(player.mapId).toBe("FFFF");
      expect(player.previousMapId).toBe("FFFF");
    });

    it.each([
      [0x0600, "0600"],
      [0x1000, "1000"],
      [0x7fff, "7FFF"],
      [-0x8000, "8000"],
    ])("should format map id %i as %s", (rawMapId, expectedMapId) => {
      const player = PlayerAssembler.assemble(createResource({ mapId: rawMapId }));

      expect(player.mapId).toBe(expectedMapId);
    });

    it("should keep negative bits when the raw value is negative", () => {
      const player = PlayerAssembler.assemble(createResource({ bits: -1 }));

      expect(player.bits).toBe(-1);
    });
  });
});
