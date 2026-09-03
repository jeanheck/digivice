import { WikiDroppedBySourceConverter } from "@/presenters/converter/wiki-dropped-by-source.converter";
import { DropRepository } from "@/repositories/drop.repository";
import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDropsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiDropsPanelPresenter {
  private static dropSourcesByDropId: Map<string, DropSourceViewModel[]> | null = null;

  public static getViewModel(dropId: string): WikiDropsPanelViewModel {
    const dropRaw = DropRepository.getDropByKey(dropId);
    const dropSources = WikiDropsPanelPresenter.getDropSourcesByDropId().get(dropId) ?? [];
    const isBooster = dropRaw?.type === "booster";

    return {
      dropType: dropRaw?.type ?? null,
      dropNumericId: dropRaw?.id ?? null,
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
        if (drop.id === VARIOUS_BOOSTER_DROP_ID) {
          continue;
        }

        const existingSources = dropSourcesByDropId.get(drop.id) ?? [];
        existingSources.push({
          kind: "enemy",
          sourceId: enemyId,
          label: enemyRaw.name,
          locationId: drop.locationOnly,
        });
        dropSourcesByDropId.set(drop.id, existingSources);
      }
    }

    for (const [tamerId, tamerRaw] of Object.entries(TamerRepository.getTamerTable())) {
      for (const cardBattle of Object.values(tamerRaw.cardBattles ?? {})) {
        WikiDropsPanelPresenter.addNpcDropSource(
          dropSourcesByDropId,
          npcSourceKeysByDropId,
          cardBattle.dropId,
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
          cardBattle.dropId,
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
