import { describe, expect, it } from "vitest";
import { CardBattleAssembler } from "@/memory/assemblers/card-battle.assembler";

describe("CardBattleAssembler", () => {
  describe("assemble", () => {
    it.each([1, 5, 0x7fffffff])("should keep the id when it is %i", (rawId) => {
      const cardBattle = CardBattleAssembler.assemble({ id: rawId });

      expect(cardBattle).toEqual({ id: rawId });
    });

    it.each([0, -1, -0x80000000])("should return a null id when it is %i", (rawId) => {
      const cardBattle = CardBattleAssembler.assemble({ id: rawId });

      expect(cardBattle).toEqual({ id: null });
    });
  });
});
