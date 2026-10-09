import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { EnemyOwnerRepository } from "@/repositories/enemy-owner.repository";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class SearchItemConverter {
  public static convertEnemy(
    id: string,
    enemyRaw: EnemyRaw,
    translate: (key: string) => string,
  ): SearchItemViewModel {
    const searchItem: SearchItemViewModel = {
      id,
      name: enemyRaw.name,
      kind: "enemy",
    };
    const levelLabel = String(enemyRaw.level);

    if (enemyRaw.boss === true) {
      searchItem.kindLabelKey = "enemy.searchContext.boss";
      searchItem.kindLabelParams = { level: levelLabel };
      return searchItem;
    }

    const ownerId = EnemyOwnerRepository.getOwnerId(enemyRaw.memoryId, enemyRaw.groupId);
    const ownerNameKey = ownerId !== null ? NpcBattleOpponentHelper.getNameKey(ownerId) : null;
    if (ownerId !== null && ownerNameKey !== null) {
      const isTamer = NpcBattleOpponentHelper.getSearchKind(ownerId) === "tamer";
      searchItem.kindLabelKey = isTamer ? "enemy.searchContext.tamer" : "enemy.searchContext.npc";
      searchItem.kindLabelParams = {
        name: translate(ownerNameKey),
        level: levelLabel,
      };
      return searchItem;
    }

    searchItem.kindLabelKey = "enemy.searchContext.wild";
    searchItem.kindLabelParams = { level: levelLabel };
    return searchItem;
  }

  public static convertEquipment(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "equipment",
    };
  }

  public static convertConsumableItem(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "consumableItem",
    };
  }

  public static convertBooster(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "booster",
    };
  }

  public static convertCard(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "card",
    };
  }

  public static convertLocation(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "location",
    };
  }

  public static convertCardShop(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "cardShop",
    };
  }

  public static convertTamer(id: string, translatedName: string): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind: "tamer",
    };
  }

  public static convertDuelIsland(id: string, translatedName: string): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind: "npc",
    };
  }

  public static convertStoryNpc(
    id: string,
    translatedName: string,
    kind: "leader" | "npc",
  ): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind,
    };
  }
}
