import { MapPresenter } from "@/presenters/map/map.presenter";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import type { CardBattleMapViewModel } from "@/viewmodels/map/card-battle-map.viewmodel";

const CARD_BATTLE_LOCATION_ID = "0700";

export class CardBattleMapPresenter {
  public static getViewModel(opponentId: number): CardBattleMapViewModel {
    const npcId = NpcBattleOpponentHelper.getIdByOpponentId(opponentId);
    const backgroundImageUrl = MapPresenter.getByLocationId(CARD_BATTLE_LOCATION_ID).locationImageUrl;

    return {
      npcId,
      titleKey: npcId !== null ? NpcBattleOpponentHelper.getNameKey(npcId) : null,
      backgroundImageUrl,
    };
  }
}
