import type { CardBattle } from "@/models/card-battle";
import type * as Events from "@/events/events.map";

export class CardBattleSyncer {
  public static sync(previousCardBattle: CardBattle, newCardBattleDto: Events.CardBattleDTO): void {
    if (newCardBattleDto.id !== undefined) {
      previousCardBattle.id = newCardBattleDto.id;
    }
  }
}
