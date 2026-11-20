import { describe, expect, it } from "vitest";
import { DigimonBattleAssembler } from "@/memory/assemblers/digimon-battle.assembler";
import type { EnemyResource } from "@/memory/resources/battles/enemy.resource";

const EnemySample: EnemyResource = {
  id: 122,
  groupId: 201,
  condition: 0x01,
  strength: 70,
  defense: 65,
  speed: 84,
  hp: { current: 600, max: 672 },
};

describe("DigimonBattleAssembler", () => {
  describe("assemble", () => {
    it("should keep the field and the full enemy when the enemy id is positive", () => {
      const digimonBattle = DigimonBattleAssembler.assemble({ field: 3, enemy: EnemySample });

      expect(digimonBattle).toEqual({
        field: 3,
        enemy: {
          id: 122,
          groupId: 201,
          condition: 0x01,
          strength: 70,
          defense: 65,
          speed: 84,
          hp: { current: 600, max: 672 },
        },
      });
    });

    it.each([0, -1, -0x8000])("should return a null enemy when the enemy id is %i", (rawId) => {
      const digimonBattle = DigimonBattleAssembler.assemble({
        field: 3,
        enemy: { ...EnemySample, id: rawId },
      });

      expect(digimonBattle).toEqual({ field: 3, enemy: null });
    });

    it("should keep the neutral field when the field is zero", () => {
      const digimonBattle = DigimonBattleAssembler.assemble({ field: 0, enemy: EnemySample });

      expect(digimonBattle.field).toBe(0);
    });

    it("should not share the hp object with the resource", () => {
      const digimonBattle = DigimonBattleAssembler.assemble({ field: 0, enemy: EnemySample });

      expect(digimonBattle.enemy?.hp).not.toBe(EnemySample.hp);
    });
  });
});
