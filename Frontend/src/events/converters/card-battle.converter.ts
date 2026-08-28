import type { CardBattleDTO } from "@/events/dto/card-battle.dto";
import type { CardBattle } from "@/models/card-battle";

export class CardBattleConverter {
  public static convert(cardBattleDto: Required<CardBattleDTO>): CardBattle {
    return {
      opponentId: cardBattleDto.opponentId ?? 0,
    };
  }
}
