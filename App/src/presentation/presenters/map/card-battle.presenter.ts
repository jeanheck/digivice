import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { MapId } from "@/shared/constants/map-id.constant";
import { NpcBattleOpponentHelper } from "@/presentation/presenters/helper/npc-battle-opponent.helper";
import { MapService } from "@/presentation/services/map.service";
import type { CardBattleViewModel } from "@/presentation/viewmodels/map/card-battle.viewmodel";

export class CardBattlePresenter {
  public static getViewModel(cardBattleId: number | null): CardBattleViewModel {
    const npcId = NpcBattleOpponentHelper.getNpcIdByCardBattleId(cardBattleId);
    const backgroundImageUrl = ImageCatalog.getLocationImageUrl(
      MapService.getImageNameByMapId(MapId.cardBattle),
    );

    return {
      npcId,
      titleKey: npcId !== null ? NpcBattleOpponentHelper.getNameKey(npcId) : null,
      backgroundImageUrl,
    };
  }
}
