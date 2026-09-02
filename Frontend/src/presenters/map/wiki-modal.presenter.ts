import { CardRepository } from "@/repositories/card.repository";
import { DropRepository } from "@/repositories/drop.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { LocationRepository } from "@/repositories/location.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import { StoreRepository } from "@/repositories/store.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { SearchItemConverter } from "@/presenters/converter/search-item.converter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { SearchItemKind, SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class WikiModalPresenter {
  public static isNpcSearchKind(kind: SearchItemKind | undefined): boolean {
    return kind === "tamer" || kind === "leader" || kind === "npc";
  }

  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }

  public static getEnemySearchItems(
    translateTamerName: (tamerId: string) => string,
    translateNpcName: (npcId: string) => string,
  ): SearchItemViewModel[] {
    return Object.entries(EnemyRepository.getEnemyTable()).map(([enemyId, enemyRaw]) => {
      return SearchItemConverter.convertEnemy(enemyId, enemyRaw, {
        translateTamerName,
        translateNpcName,
      });
    });
  }

  public static getDropSearchItems(translateDropName: (dropKey: string) => string): SearchItemViewModel[] {
    return DropRepository.getDropKeys().map((dropKey) => {
      return SearchItemConverter.convertDrop(dropKey, translateDropName(dropKey));
    });
  }

  public static getCardSearchItems(translateCardName: (cardId: string) => string): SearchItemViewModel[] {
    return CardRepository.getCardIds().map((cardId) => {
      return SearchItemConverter.convertCard(cardId, translateCardName(cardId));
    });
  }

  public static getLocationSearchItems(
    translateLocationName: (locationId: string) => string,
  ): SearchItemViewModel[] {
    return LocationRepository.getLocationIdsWithWorldLocation().map((locationId) => {
      return SearchItemConverter.convertLocation(locationId, translateLocationName(locationId));
    });
  }

  public static getStoreSearchItems(translateStoreName: (storeId: string) => string): SearchItemViewModel[] {
    return StoreRepository.getStoreIds().map((storeId) => {
      return SearchItemConverter.convertStore(storeId, translateStoreName(storeId));
    });
  }

  public static getTamerSearchItems(
    translateTamerName: (tamerId: string) => string,
  ): SearchItemViewModel[] {
    return Object.entries(TamerRepository.getTamerTable()).map(([tamerId]) => {
      return SearchItemConverter.convertTamer(tamerId, translateTamerName(tamerId));
    });
  }

  public static getDuelIslandSearchItems(
    translateDuelIslandName: (duelIslandId: string) => string,
  ): SearchItemViewModel[] {
    return Object.entries(DuelIslandRepository.getDuelIslandTable()).map(([duelIslandId]) => {
      return SearchItemConverter.convertDuelIsland(
        duelIslandId,
        translateDuelIslandName(duelIslandId),
      );
    });
  }

  public static getStoryNpcSearchItems(
    translateNpcName: (npcId: string) => string,
  ): SearchItemViewModel[] {
    return Object.entries(NpcRepository.getNpcTable()).map(([npcId, npcRaw]) => {
      return SearchItemConverter.convertStoryNpc(npcId, translateNpcName(npcId), npcRaw.type);
    });
  }

  public static getAllSearchItems(
    translateDropName: (dropKey: string) => string,
    translateCardName: (cardId: string) => string,
    translateLocationName: (locationId: string) => string,
    translateStoreName: (storeId: string) => string,
    translateTamerName: (tamerId: string) => string,
    translateDuelIslandName: (duelIslandId: string) => string,
    translateNpcName: (npcId: string) => string,
  ): SearchItemViewModel[] {
    return [
      ...this.getEnemySearchItems(translateTamerName, translateNpcName),
      ...this.getDropSearchItems(translateDropName),
      ...this.getCardSearchItems(translateCardName),
      ...this.getLocationSearchItems(translateLocationName),
      ...this.getStoreSearchItems(translateStoreName),
      ...this.getTamerSearchItems(translateTamerName),
      ...this.getDuelIslandSearchItems(translateDuelIslandName),
      ...this.getStoryNpcSearchItems(translateNpcName),
    ];
  }
}
