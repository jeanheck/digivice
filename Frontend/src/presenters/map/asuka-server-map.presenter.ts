import type { DigimonSlot, ImportantItems, Npc, Quest } from "@/models";
import { AsukaServerMapConverter } from "@/presenters/converter/asuka-server-map.converter";
import { FooterPresenter } from "@/presenters/footer/footer.presenter";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { QuestRepository } from "@/repositories/quest.repository";
import { LocationService } from "@/services/location.service";
import { NpcService } from "@/services/npc.service";
import { QuestService } from "@/services/quest.service";
import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";
import type { MapNpcViewModel } from "@/viewmodels/map/map-npc.viewmodel";

export class AsukaServerMapPresenter {
  private static readonly ASUKA_SEWERS_LOCATION_ID = "021B";
  private static readonly UNDERGROUND_PATH_LOCATION_ID = "020B";
  private static readonly FISHING_POLE_QUEST_ID = "fishingPole";
  private static readonly TREE_BOOTS_QUEST_ID = "treeBoots";

  private static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return (
      locationId === this.ASUKA_SEWERS_LOCATION_ID &&
      previousMapId === this.UNDERGROUND_PATH_LOCATION_ID
    );
  }

  private static resolveFishingIds(locationId: string, sideQuests: Quest[]): string[] {
    const fishingPoleQuest = sideQuests.find((quest) => {
      return quest.id === this.FISHING_POLE_QUEST_ID;
    });
    const fishingPoleRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
      return questRaw.id === this.FISHING_POLE_QUEST_ID;
    });

    if (
      fishingPoleRaw === undefined ||
      !QuestService.isQuestCompleted(fishingPoleQuest, fishingPoleRaw)
    ) {
      return [];
    }

    return LocationService.getFishing(locationId);
  }

  private static resolveKickingTreeIds(locationId: string, sideQuests: Quest[]): string[] {
    const treeBootsQuest = sideQuests.find((quest) => {
      return quest.id === this.TREE_BOOTS_QUEST_ID;
    });
    const treeBootsRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
      return questRaw.id === this.TREE_BOOTS_QUEST_ID;
    });

    if (
      treeBootsRaw === undefined ||
      !QuestService.isQuestCompleted(treeBootsQuest, treeBootsRaw)
    ) {
      return [];
    }

    return LocationService.getKickingTree(locationId);
  }

  private static resolveNpcs(
    locationId: string,
    lastCompletedMainQuestStep: number,
    digimonSlots: DigimonSlot[],
    journalNpcs: Npc[],
    importantItems: ImportantItems | null | undefined,
  ): MapNpcViewModel[] {
    const opponentIds = LocationService.getMapOpponentIds(
      locationId,
      lastCompletedMainQuestStep,
    );
    const partyCharisma = FooterPresenter.getPartyCharisma(digimonSlots);

    return opponentIds.flatMap((opponentId) => {
      const opponent = NpcBattleOpponentHelper.resolveById(opponentId);
      const nameKey = NpcBattleOpponentHelper.getNameKey(opponentId);
      if (opponent === undefined || nameKey === null) {
        return [];
      }

      const journalNpc =
        journalNpcs.find((npc) => {
          return npc.id === opponentId;
        }) ?? null;

      const availableBattleKind = NpcService.getAvailableBattleKindForOpponent(
        opponent,
        journalNpc,
        partyCharisma,
        importantItems,
      );

      return [
        {
          id: opponentId,
          nameKey,
          hasAvailableBattle: availableBattleKind !== null,
          availableBattleKind,
        },
      ];
    });
  }

  public static getViewModel(
    locationId: string,
    mainQuest: Quest | null,
    sideQuests: Quest[],
    digimonSlots: DigimonSlot[],
    previousMapId: string = "",
    journalNpcs: Npc[] = [],
    importantItems: ImportantItems | null | undefined = null,
  ): AsukaServerMapViewModel {
    const fishingIds = this.resolveFishingIds(locationId, sideQuests);
    const kickingTreeIds = this.resolveKickingTreeIds(locationId, sideQuests);
    const bossIds = LocationService.getBoss(locationId);
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const npcs = this.resolveNpcs(
      locationId,
      lastCompletedMainQuestStep,
      digimonSlots,
      journalNpcs,
      importantItems,
    );

    if (this.isAsukaSewersSafeZone(locationId, previousMapId)) {
      return AsukaServerMapConverter.convert(
        locationId,
        [],
        bossIds,
        fishingIds,
        kickingTreeIds,
        npcs,
      );
    }

    const enemyIds = LocationService.getEnemies(locationId, lastCompletedMainQuestStep);

    return AsukaServerMapConverter.convert(
      locationId,
      enemyIds,
      bossIds,
      fishingIds,
      kickingTreeIds,
      npcs,
    );
  }
}
