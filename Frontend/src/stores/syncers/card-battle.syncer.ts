import type { CardBattle } from "@/models";
import type { CardBattleDTO } from "@/events/dto/card-battle.dto";

export class CardBattleSyncer {
  public static sync(previousCardBattle: CardBattle, newCardBattleDto: CardBattleDTO): void {
    if (newCardBattleDto.id !== undefined) {
      previousCardBattle.id = newCardBattleDto.id;
    }
  }
}
