import { ImageCatalog } from "@/catalogs/image.catalog";
import { MapIdConstant } from "@/constants/map-id.constant";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { LocationService } from "@/services/location.service";
import type { CardBattleViewModel } from "@/viewmodels/map/card-battle.viewmodel";

export class CardBattlePresenter {
  public static getViewModel(cardBattleId: number | null): CardBattleViewModel {
    const npcId = NpcBattleOpponentHelper.getNpcIdByCardBattleId(cardBattleId);
    const backgroundImageUrl = ImageCatalog.getLocationImageUrl(
      LocationService.getLocationImageNameByLocationId(MapIdConstant.cardBattle),
    );

    return {
      npcId,
      titleKey: npcId !== null ? NpcBattleOpponentHelper.getNameKey(npcId) : null,
      backgroundImageUrl,
    };
  }
}
