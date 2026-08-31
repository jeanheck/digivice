import { MapPresenter } from "@/presenters/map/map.presenter";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import type { CardBattleMapViewModel } from "@/viewmodels/map/card-battle-map.viewmodel";

export class CardBattleMapPresenter {
  private static readonly cardBattleLocationId = "0700";

  public static isInCardBattle(locationId: string | null): boolean {
    return locationId === this.cardBattleLocationId;
  }

  public static getViewModel(opponentId: number): CardBattleMapViewModel {
    const npcId = NpcBattleOpponentHelper.getIdByOpponentId(opponentId);
    const backgroundImageUrl = MapPresenter.getByLocationId(this.cardBattleLocationId).locationImageUrl;

    return {
      npcId,
      titleKey: npcId !== null ? NpcBattleOpponentHelper.getNameKey(npcId) : null,
      backgroundImageUrl,
    };
  }
}
