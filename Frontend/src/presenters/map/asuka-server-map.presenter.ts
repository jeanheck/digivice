import type { DigimonSlot, ImportantItems, Npc, Quest } from "@/models";
import { AsukaServerMapConverter } from "@/presenters/converter/asuka-server-map.converter";
import { FooterPresenter } from "@/presenters/footer/footer.presenter";
import { LocationEncounterHelper } from "@/presenters/helper/location-encounter.helper";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { LocationService } from "@/services/location.service";
import { NpcService } from "@/services/npc.service";
import { QuestService } from "@/services/quest.service";
import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";
import type { MapNpcViewModel } from "@/viewmodels/map/map-npc.viewmodel";

export class AsukaServerMapPresenter {
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
    const fishingIds = LocationEncounterHelper.resolveFishingIds(locationId, sideQuests);
    const kickingTreeIds = LocationEncounterHelper.resolveKickingTreeIds(locationId, sideQuests);
    const bossIds = LocationService.getBoss(locationId);
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const npcs = this.resolveNpcs(
      locationId,
      lastCompletedMainQuestStep,
      digimonSlots,
      journalNpcs,
      importantItems,
    );
    const enemyIds = LocationEncounterHelper.resolveWalkingIds(
      locationId,
      mainQuest,
      previousMapId,
    );

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
