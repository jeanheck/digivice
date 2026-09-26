import type { DeepRequired } from "@/events/dto/deep-required";
import type { CardBattleDTO } from "@/events/dto/card-battle.dto";
import type { CardBattle } from "@/models";

export class CardBattleConverter {
  public static convert(cardBattleDto: DeepRequired<CardBattleDTO>): CardBattle {
    return {
      id: cardBattleDto.id,
    };
  }
}
