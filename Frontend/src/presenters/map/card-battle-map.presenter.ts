import { MapPresenter } from "@/presenters/map/map.presenter";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { CardBattleMapViewModel } from "@/viewmodels/map/card-battle-map.viewmodel";

export class CardBattleMapPresenter {
  private static readonly cardBattleLocationId = "0700";

  public static isInCardBattle(locationId: string | null): boolean {
    return locationId === this.cardBattleLocationId;
  }

  public static getViewModel(opponentId: number): CardBattleMapViewModel {
    const tamerId = TamerRepository.getTamerIdByOpponentId(opponentId);
    const backgroundImageUrl = MapPresenter.getByLocationId(this.cardBattleLocationId).locationImageUrl;

    return {
      npcId: tamerId,
      titleKey: tamerId !== null ? `tamers.${tamerId}.name` : null,
      backgroundImageUrl,
    };
  }
}
