import { WikiDroppedBySourceConverter } from "@/presenters/converter/wiki-dropped-by-source.converter";
import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDropsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";

export class WikiDropsPanelPresenter {
  private static dropSourcesByDropId: Map<string, DropSourceViewModel[]> | null = null;

  public static getViewModel(dropId: string, dropType: DropType): WikiDropsPanelViewModel {
    const dropSources = WikiDropsPanelPresenter.getDropSourcesByDropId().get(dropId) ?? [];
    const isBooster = dropType === "booster";
    const dropNumericId = Number(dropId);

    return {
      dropType,
      dropNumericId,
      sources: dropSources.map((dropSource) => {
        return WikiDroppedBySourceConverter.convert(dropSource);
      }),
      sourcesSectionLabelKey: isBooster ? "enemy.obtainedFrom" : "enemy.droppedBy",
      sourcesEmptyLabelKey: isBooster ? "enemy.dropSourcesNone" : "enemy.droppedByNone",
    };
  }

  private static getDropSourcesByDropId(): Map<string, DropSourceViewModel[]> {
    if (this.dropSourcesByDropId !== null) {
      return this.dropSourcesByDropId;
    }

    const dropSourcesByDropId = new Map<string, DropSourceViewModel[]>();
    const npcSourceKeysByDropId = new Map<string, Set<string>>();

    for (const [enemyId, enemyRaw] of Object.entries(EnemyRepository.getEnemyTable())) {
      for (const drop of enemyRaw.drops ?? []) {
        const dropKey = String(drop.dropId);
        const existingSources = dropSourcesByDropId.get(dropKey) ?? [];
        existingSources.push({
          kind: "enemy",
          sourceId: enemyId,
          label: enemyRaw.name,
          locationId: drop.locationOnly,
        });
        dropSourcesByDropId.set(dropKey, existingSources);
      }
    }

    for (const [tamerId, tamerRaw] of Object.entries(TamerRepository.getTamerTable())) {
      for (const cardBattle of Object.values(tamerRaw.cardBattles ?? {})) {
        WikiDropsPanelPresenter.addNpcDropSource(
          dropSourcesByDropId,
          npcSourceKeysByDropId,
          String(cardBattle.boosterId),
          {
            kind: "tamer",
            sourceId: tamerId,
            labelKey: `tamers.${tamerId}.name`,
          },
        );
      }
    }

    for (const [duelIslandId, duelIslandRaw] of Object.entries(DuelIslandRepository.getDuelIslandTable())) {
      for (const cardBattle of Object.values(duelIslandRaw.cardBattles ?? {})) {
        WikiDropsPanelPresenter.addNpcDropSource(
          dropSourcesByDropId,
          npcSourceKeysByDropId,
          String(cardBattle.boosterId),
          {
            kind: "duelIsland",
            sourceId: duelIslandId,
            labelKey: `duelIsland.${duelIslandId}.name`,
          },
        );
      }
    }

    this.dropSourcesByDropId = dropSourcesByDropId;
    return this.dropSourcesByDropId;
  }

  private static addNpcDropSource(
    dropSourcesByDropId: Map<string, DropSourceViewModel[]>,
    npcSourceKeysByDropId: Map<string, Set<string>>,
    dropId: string,
    dropSource: DropSourceViewModel,
  ): void {
    const npcSourceKey = `${dropSource.kind}:${dropSource.sourceId}`;
    const existingNpcKeys = npcSourceKeysByDropId.get(dropId) ?? new Set<string>();

    if (existingNpcKeys.has(npcSourceKey)) {
      return;
    }

    existingNpcKeys.add(npcSourceKey);
    npcSourceKeysByDropId.set(dropId, existingNpcKeys);

    const existingSources = dropSourcesByDropId.get(dropId) ?? [];
    existingSources.push(dropSource);
    dropSourcesByDropId.set(dropId, existingSources);
  }
}
