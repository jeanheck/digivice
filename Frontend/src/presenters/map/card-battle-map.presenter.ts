import { MapPresenter } from "@/presenters/map/map.presenter";
import { NpcRepository } from "@/repositories/npc.repository";
import type { CardBattleMapViewModel } from "@/viewmodels/map/card-battle-map.viewmodel";

export class CardBattleMapPresenter {
  private static readonly cardBattleLocationId = "0700";

  public static isInCardBattle(locationId: string | null): boolean {
    return locationId === this.cardBattleLocationId;
  }

  public static getViewModel(opponentId: number): CardBattleMapViewModel {
    const npcId = NpcRepository.getNpcIdByOpponentId(opponentId);
    const npcRaw = npcId !== null ? NpcRepository.getNpcById(npcId) : undefined;
    const backgroundImageUrl = MapPresenter.getByLocationId(this.cardBattleLocationId).locationImageUrl;

    return {
      npcId,
      title: npcRaw?.name ?? "",
      backgroundImageUrl,
    };
  }
}
