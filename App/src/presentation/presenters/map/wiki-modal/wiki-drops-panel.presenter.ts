import { WikiDroppedBySourceConverter } from "@/presentation/presenters/converter/wiki-dropped-by-source.converter";
import { NpcBattleOpponentHelper } from "@/presentation/presenters/helper/npc-battle-opponent.helper";
import { DuelIslandRepository } from "@/presentation/repositories/duel-island.repository";
import { EnemyOwnerRepository } from "@/presentation/repositories/enemy-owner.repository";
import { EnemyRepository } from "@/presentation/repositories/enemy.repository";
import { TamerRepository } from "@/presentation/repositories/tamer.repository";
import { EquipmentShopRepository } from "@/presentation/repositories/equipment-shop.repository";
import { ItemShopRepository } from "@/presentation/repositories/item-shop.repository";
import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";
import type { DropSourceKind } from "@/shared/types/drop-source-kind.type";
import type { DropSourceViewModel } from "@/presentation/viewmodels/drop/drop-source.viewmodel";
import type {
  PurchasableShopKind,
  WikiDropsPanelViewModel,
} from "@/presentation/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";
interface DroppedByRaw {
  kind: DropSourceKind;
  id: string;
  locationOnly?: string;
}

export class WikiDropsPanelPresenter {
  public static getViewModel(dropId: string, dropType: DropType): WikiDropsPanelViewModel {
    const isBooster = dropType === "booster";
    const dropNumericId = Number(dropId);

    const sources = this.getDroppedByRaw(dropId, dropType).map((droppedBy) => {
      return WikiDroppedBySourceConverter.convert(this.toDropSource(droppedBy));
    });

    sources.sort((firstSource, secondSource) => {
      const byLabel = (firstSource.label ?? "").localeCompare(secondSource.label ?? "");
      if (byLabel !== 0) {
        return byLabel;
      }

      return firstSource.sourceId.localeCompare(secondSource.sourceId);
    });

    return {
      dropType,
      dropNumericId,
      sources,
      sourcesSectionLabelKey: "enemy.droppedBy",
      sourcesEmptyLabelKey: isBooster ? "enemy.dropSourcesNone" : "enemy.droppedByNone",
      purchasableShopKind: this.getPurchasableShopKind(dropType),
      purchasableShopIds: this.getPurchasableShopIds(dropId, dropType),
    };
  }

  private static getPurchasableShopKind(dropType: DropType): PurchasableShopKind | null {
    if (dropType === "equipment") {
      return "equipmentShop";
    }

    if (dropType === "consumableItem") {
      return "itemShop";
    }

    return null;
  }

  private static getPurchasableShopIds(dropId: string, dropType: DropType): string[] {
    if (dropType === "equipment") {
      return EquipmentShopRepository.getEquipmentShopIdsByEquipmentId(dropId);
    }

    if (dropType === "consumableItem") {
      return ItemShopRepository.getItemShopIdsByConsumableItemId(dropId);
    }

    return [];
  }

  private static getDroppedByRaw(dropId: string, dropType: DropType): DroppedByRaw[] {
    const dropNumericId = Number(dropId);
    const enemiesWhoDrop: DroppedByRaw[] = EnemyRepository.getEnemiesWhoDropByDrop(
      dropType,
      dropNumericId,
    ).map((enemyWhoDrops) => {
      return {
        kind: "enemy",
        id: enemyWhoDrops.enemyId,
        locationOnly: enemyWhoDrops.locationOnly,
      };
    });

    if (dropType !== "booster") {
      return enemiesWhoDrop;
    }

    const tamersWhoDrop: DroppedByRaw[] = TamerRepository.getTamersWhoDropByBooster(
      dropNumericId,
    ).map((tamerId) => {
      return { kind: "tamer", id: tamerId };
    });

    const duelIslandsWhoDrop: DroppedByRaw[] = DuelIslandRepository.getDuelIslandsWhoDropByBooster(
      dropNumericId,
    ).map((duelIslandId) => {
      return { kind: "duelIsland", id: duelIslandId };
    });

    return [...enemiesWhoDrop, ...tamersWhoDrop, ...duelIslandsWhoDrop];
  }

  private static toDropSource(droppedBy: DroppedByRaw): DropSourceViewModel {
    if (droppedBy.kind === "enemy") {
      const enemyRaw = EnemyRepository.getEnemyById(droppedBy.id);
      const ownerId = EnemyOwnerRepository.getOwnerId(enemyRaw.memoryId, enemyRaw.groupId);
      const ownerNameKey = ownerId !== null ? NpcBattleOpponentHelper.getNameKey(ownerId) : null;

      return {
        kind: "enemy",
        sourceId: droppedBy.id,
        label: enemyRaw.name,
        locationId: droppedBy.locationOnly,
        ownerNameKey: ownerNameKey ?? undefined,
      };
    }

    if (droppedBy.kind === "tamer") {
      return {
        kind: "tamer",
        sourceId: droppedBy.id,
        labelKey: `tamers.${droppedBy.id}.name`,
      };
    }

    return {
      kind: "duelIsland",
      sourceId: droppedBy.id,
      labelKey: `duelIsland.${droppedBy.id}.name`,
    };
  }
}
