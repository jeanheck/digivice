import type { DigimonSlot, ImportantItems, Journal, Npc, Npcs } from "@/state/models";
import { AsukaServerMapConverter } from "@/presentation/presenters/converter/asuka-server-map.converter";
import { LocationEncounterHelper } from "@/presentation/presenters/helper/location-encounter.helper";
import { NpcBattleOpponentHelper } from "@/presentation/presenters/helper/npc-battle-opponent.helper";
import { MapService } from "@/presentation/services/map.service";
import { NpcService } from "@/presentation/services/npc.service";
import { PartyService } from "@/presentation/services/party.service";
import { QuestService } from "@/presentation/services/quest.service";
import type { AsukaServerMapViewModel } from "@/presentation/viewmodels/map/asuka-server-map.viewmodel";
import type { MapNpcViewModel } from "@/presentation/viewmodels/map/map-npc.viewmodel";

export class AsukaServerMapPresenter {
  private static resolveNpcs(
    locationId: string,
    lastCompletedMainQuestStep: number,
    digimonSlots: DigimonSlot[],
    npcs: Npcs,
    importantItems: ImportantItems,
  ): MapNpcViewModel[] {
    const opponentIds = MapService.getMapOpponentIds(locationId, lastCompletedMainQuestStep);
    const partyCharisma = PartyService.getCharisma({ slots: digimonSlots });

    return opponentIds.flatMap((opponentId) => {
      const opponent = NpcBattleOpponentHelper.resolveById(opponentId);
      const nameKey = NpcBattleOpponentHelper.getNameKey(opponentId);
      if (opponent === undefined || nameKey === null) {
        return [];
      }

      const journalNpc = this.resolveNpc(npcs, opponentId);

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

  private static resolveNpc(npcs: Npcs, npcId: string): Npc | null {
    return npcs[npcId as keyof Npcs] ?? null;
  }

  public static getViewModel(
    locationId: string,
    journal: Journal,
    digimonSlots: DigimonSlot[],
    previousMapId: string,
    npcs: Npcs,
    importantItems: ImportantItems,
  ): AsukaServerMapViewModel {
    const fishingIds = LocationEncounterHelper.resolveFishingIds(locationId, journal.sideQuests);
    const kickingTreeIds = LocationEncounterHelper.resolveKickingTreeIds(
      locationId,
      journal.sideQuests,
    );
    const bossIds = MapService.getBoss(locationId, journal);
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(
      journal.mainQuest,
    );
    const mapNpcs = this.resolveNpcs(
      locationId,
      lastCompletedMainQuestStep,
      digimonSlots,
      npcs,
      importantItems,
    );
    const enemyIds = LocationEncounterHelper.resolveWalkingIds(
      locationId,
      journal.mainQuest,
      previousMapId,
    );

    return AsukaServerMapConverter.convert(
      locationId,
      enemyIds,
      bossIds,
      fishingIds,
      kickingTreeIds,
      mapNpcs,
    );
  }
}
