import { CardRepository } from "@/repositories/card.repository";
import { BoosterRepository } from "@/repositories/booster.repository";
import { ConsumableItemRepository } from "@/repositories/consumable-item.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { LocationRepository } from "@/repositories/location.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import {
  NpcBattleFromEnemyHelper,
  type NpcBattleFromEnemyContext,
} from "@/presenters/helper/npc-battle-from-enemy.helper";
import { SearchItemConverter } from "@/presenters/converter/search-item.converter";
import { DropService } from "@/services/drop.service";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { SearchItemKind, SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class WikiModalPresenter {
  public static isNpcSearchKind(kind: SearchItemKind | undefined): boolean {
    return kind === "tamer" || kind === "leader" || kind === "npc";
  }

  public static isDropSearchKind(kind: SearchItemKind | undefined): kind is DropType {
    return kind === "equipment" || kind === "consumableItem" || kind === "booster";
  }

  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }

  public static resolveNpcBattleFromEnemyId(
    enemyId: string,
  ): NpcBattleFromEnemyContext | null {
    return NpcBattleFromEnemyHelper.resolve(enemyId);
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

  public static getDropSearchItems(translateLabelKey: (labelKey: string) => string): SearchItemViewModel[] {
    const equipmentItems = EquipmentRepository.getIds().map((equipmentId) => {
      return SearchItemConverter.convertEquipment(
        equipmentId,
        translateLabelKey(DropService.getDropTranslationKeyById(equipmentId, "equipment")),
      );
    });

    const consumableItems = ConsumableItemRepository.getIds().map((consumableItemId) => {
      return SearchItemConverter.convertConsumableItem(
        consumableItemId,
        translateLabelKey(
          DropService.getDropTranslationKeyById(consumableItemId, "consumableItem"),
        ),
      );
    });

    const boosterItems = BoosterRepository.getIds().map((boosterId) => {
      return SearchItemConverter.convertBooster(
        boosterId,
        translateLabelKey(DropService.getDropTranslationKeyById(boosterId, "booster")),
      );
    });

    return [...equipmentItems, ...consumableItems, ...boosterItems];
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

  public static getCardShopSearchItems(
    translateCardShopName: (cardShopId: string) => string,
  ): SearchItemViewModel[] {
    return CardShopRepository.getIds().map((cardShopId) => {
      return SearchItemConverter.convertCardShop(cardShopId, translateCardShopName(cardShopId));
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
    translateLabelKey: (labelKey: string) => string,
    translateCardName: (cardId: string) => string,
    translateLocationName: (locationId: string) => string,
    translateCardShopName: (cardShopId: string) => string,
    translateTamerName: (tamerId: string) => string,
    translateDuelIslandName: (duelIslandId: string) => string,
    translateNpcName: (npcId: string) => string,
  ): SearchItemViewModel[] {
    return [
      ...this.getEnemySearchItems(translateTamerName, translateNpcName),
      ...this.getDropSearchItems(translateLabelKey),
      ...this.getCardSearchItems(translateCardName),
      ...this.getLocationSearchItems(translateLocationName),
      ...this.getCardShopSearchItems(translateCardShopName),
      ...this.getTamerSearchItems(translateTamerName),
      ...this.getDuelIslandSearchItems(translateDuelIslandName),
      ...this.getStoryNpcSearchItems(translateNpcName),
    ];
  }
}
