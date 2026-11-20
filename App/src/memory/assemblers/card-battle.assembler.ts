import type { CardBattle } from "@/state/models";
import type { CardBattleResource } from "@/memory/resources/card-battle.resource";

export class CardBattleAssembler {
  public static assemble(resource: CardBattleResource): CardBattle {
    return {
      id: resource.id <= 0 ? null : resource.id,
    };
  }
}
