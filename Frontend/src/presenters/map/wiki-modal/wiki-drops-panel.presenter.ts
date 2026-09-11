import { WikiDroppedBySourceConverter } from "@/presenters/converter/wiki-dropped-by-source.converter";
import { BoosterRepository } from "@/repositories/booster.repository";
import { ConsumableItemRepository } from "@/repositories/consumable-item.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { EquipmentDroppedByRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { ConsumableItemDroppedByRaw } from "@/repositories/tables/raws/consumable-item/consumable-item.raw";
import type { BoosterDroppedByRaw } from "@/repositories/tables/raws/tcg/booster.raw";
import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDropsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";

type DroppedByRaw = EquipmentDroppedByRaw | ConsumableItemDroppedByRaw | BoosterDroppedByRaw;

export class WikiDropsPanelPresenter {
  public static getViewModel(dropId: string, dropType: DropType): WikiDropsPanelViewModel {
    const isBooster = dropType === "booster";
    const dropNumericId = Number(dropId);

    return {
      dropType,
      dropNumericId,
      sources: this.getDroppedByRaw(dropId, dropType).map((droppedBy) => {
        return WikiDroppedBySourceConverter.convert(this.toDropSource(droppedBy));
      }),
      sourcesSectionLabelKey: "enemy.droppedBy",
      sourcesEmptyLabelKey: isBooster ? "enemy.dropSourcesNone" : "enemy.droppedByNone",
    };
  }

  private static getDroppedByRaw(dropId: string, dropType: DropType): DroppedByRaw[] {
    if (dropType === "equipment") {
      return EquipmentRepository.getById(dropId)?.droppedBy ?? [];
    }

    if (dropType === "consumableItem") {
      return ConsumableItemRepository.getById(Number(dropId))?.droppedBy ?? [];
    }

    return BoosterRepository.getById(Number(dropId))?.droppedBy ?? [];
  }

  private static toDropSource(droppedBy: DroppedByRaw): DropSourceViewModel {
    if (droppedBy.kind === "enemy") {
      const enemyRaw = EnemyRepository.getEnemyById(droppedBy.id);

      return {
        kind: "enemy",
        sourceId: droppedBy.id,
        label: enemyRaw.name,
        locationId: droppedBy.locationOnly,
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
