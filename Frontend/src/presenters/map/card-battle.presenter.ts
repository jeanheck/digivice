import { ImageCatalog } from "@/catalogs/image.catalog";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { LocationService } from "@/services/location.service";
import type { CardBattleViewModel } from "@/viewmodels/map/card-battle.viewmodel";

const CARD_BATTLE_LOCATION_ID = "0700";

export class CardBattlePresenter {
  public static getViewModel(opponentId: number): CardBattleViewModel {
    const npcId = NpcBattleOpponentHelper.getIdByOpponentId(opponentId);
    const backgroundImageUrl = ImageCatalog.getLocationImageUrl(
      LocationService.getLocationImageNameByLocationId(CARD_BATTLE_LOCATION_ID),
    );

    return {
      npcId,
      titleKey: npcId !== null ? NpcBattleOpponentHelper.getNameKey(npcId) : null,
      backgroundImageUrl,
    };
  }
}
