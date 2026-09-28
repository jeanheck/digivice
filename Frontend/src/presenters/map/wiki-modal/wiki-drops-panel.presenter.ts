import { WikiDroppedBySourceConverter } from "@/presenters/converter/wiki-dropped-by-source.converter";
import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { DropSourceKind } from "@/types/drop-source-kind.type";
import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDropsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";

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
    };
  }

  private static getDroppedByRaw(dropId: string, dropType: DropType): DroppedByRaw[] {
    const dropNumericId = Number(dropId);
    const enemiesWhoDrop: DroppedByRaw[] = EnemyRepository.getEnemiesWhoDropByDrop(dropType, dropNumericId).map(
      (enemyWhoDrops) => {
        return {
          kind: "enemy",
          id: enemyWhoDrops.enemyId,
          locationOnly: enemyWhoDrops.locationOnly,
        };
      },
    );

    if (dropType !== "booster") {
      return enemiesWhoDrop;
    }

    const tamersWhoDrop: DroppedByRaw[] = TamerRepository.getTamersWhoDropByBooster(dropNumericId).map((tamerId) => {
      return { kind: "tamer", id: tamerId };
    });

    const duelIslandsWhoDrop: DroppedByRaw[] = DuelIslandRepository.getDuelIslandsWhoDropByBooster(dropNumericId).map(
      (duelIslandId) => {
        return { kind: "duelIsland", id: duelIslandId };
      },
    );

    return [...enemiesWhoDrop, ...tamersWhoDrop, ...duelIslandsWhoDrop];
  }

  private static toDropSource(droppedBy: DroppedByRaw): DropSourceViewModel {
    if (droppedBy.kind === "enemy") {
      const enemyRaw = EnemyRepository.getEnemyById(droppedBy.id);

      return {
        kind: "enemy",
        sourceId: droppedBy.id,
        label: enemyRaw.name,
        locationId: droppedBy.locationOnly,
        tamerId: enemyRaw.tamerId,
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
