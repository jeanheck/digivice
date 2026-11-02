import { CardRepository } from "@/presentation/repositories/card.repository";
import { BoosterRepository } from "@/presentation/repositories/booster.repository";
import { ConsumableItemRepository } from "@/presentation/repositories/consumable-item.repository";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import { EnemyRepository } from "@/presentation/repositories/enemy.repository";
import { MapRepository } from "@/presentation/repositories/map.repository";
import { NpcRepository } from "@/presentation/repositories/npc.repository";
import { DuelIslandRepository } from "@/presentation/repositories/duel-island.repository";
import { TamerRepository } from "@/presentation/repositories/tamer.repository";
import { CardShopRepository } from "@/presentation/repositories/card-shop.repository";
import { EquipmentShopRepository } from "@/presentation/repositories/equipment-shop.repository";
import { ItemShopRepository } from "@/presentation/repositories/item-shop.repository";
import { EnemyConverter } from "@/presentation/presenters/converter/enemy.converter";
import {
  NpcBattleFromEnemyHelper,
  type NpcBattleFromEnemyContext,
} from "@/presentation/presenters/helper/npc-battle-from-enemy.helper";
import { SearchItemConverter } from "@/presentation/presenters/converter/search-item.converter";
import { DropService } from "@/presentation/services/drop.service";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";
import type {
  SearchItemKind,
  SearchItemViewModel,
} from "@/presentation/viewmodels/search/search-item.viewmodel";

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

  public static resolveNpcBattleFromEnemyId(enemyId: string): NpcBattleFromEnemyContext | null {
    return NpcBattleFromEnemyHelper.resolve(enemyId);
  }

  public static getEnemySearchItems(
    translateLabelKey: (labelKey: string) => string,
  ): SearchItemViewModel[] {
    return Object.entries(EnemyRepository.getEnemyTable()).map(([enemyId, enemyRaw]) => {
      return SearchItemConverter.convertEnemy(enemyId, enemyRaw, translateLabelKey);
    });
  }

  public static getDropSearchItems(
    translateLabelKey: (labelKey: string) => string,
  ): SearchItemViewModel[] {
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

  public static getCardSearchItems(
    translateCardName: (cardId: string) => string,
  ): SearchItemViewModel[] {
    return CardRepository.getCardIds().map((cardId) => {
      return SearchItemConverter.convertCard(cardId, translateCardName(cardId));
    });
  }

  public static getLocationSearchItems(
    translateLocationName: (locationId: string) => string,
  ): SearchItemViewModel[] {
    return MapRepository.getMapIdsWithWorldLocation().map((locationId) => {
      return SearchItemConverter.convertLocation(locationId, translateLocationName(locationId));
    });
  }

  public static getCardShopSearchItems(
    translateCardShopName: (cardShopId: string) => string,
    translateLocationName: (locationId: string) => string,
  ): SearchItemViewModel[] {
    return CardShopRepository.getIds().map((cardShopId) => {
      const locationId = CardShopRepository.getById(cardShopId)?.locationId;
      const locationName = locationId !== undefined ? translateLocationName(locationId) : null;

      return SearchItemConverter.convertCardShop(
        cardShopId,
        translateCardShopName(cardShopId),
        locationName,
      );
    });
  }

  public static getEquipmentShopSearchItems(
    translateEquipmentShopName: (equipmentShopId: string) => string,
  ): SearchItemViewModel[] {
    return EquipmentShopRepository.getIds().map((equipmentShopId) => {
      return SearchItemConverter.convertEquipmentShop(
        equipmentShopId,
        translateEquipmentShopName(equipmentShopId),
      );
    });
  }

  public static getItemShopSearchItems(
    translateItemShopName: (itemShopId: string) => string,
  ): SearchItemViewModel[] {
    return ItemShopRepository.getIds().map((itemShopId) => {
      return SearchItemConverter.convertItemShop(itemShopId, translateItemShopName(itemShopId));
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
    translateEquipmentShopName: (equipmentShopId: string) => string,
    translateItemShopName: (itemShopId: string) => string,
    translateTamerName: (tamerId: string) => string,
    translateDuelIslandName: (duelIslandId: string) => string,
    translateNpcName: (npcId: string) => string,
  ): SearchItemViewModel[] {
    return [
      ...this.getEnemySearchItems(translateLabelKey),
      ...this.getDropSearchItems(translateLabelKey),
      ...this.getCardSearchItems(translateCardName),
      ...this.getLocationSearchItems(translateLocationName),
      ...this.getCardShopSearchItems(translateCardShopName, translateLocationName),
      ...this.getEquipmentShopSearchItems(translateEquipmentShopName),
      ...this.getItemShopSearchItems(translateItemShopName),
      ...this.getTamerSearchItems(translateTamerName),
      ...this.getDuelIslandSearchItems(translateDuelIslandName),
      ...this.getStoryNpcSearchItems(translateNpcName),
    ];
  }
}
