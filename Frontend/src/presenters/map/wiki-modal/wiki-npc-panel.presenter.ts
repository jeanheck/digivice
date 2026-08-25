import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import { NpcRepository } from "@/repositories/npc.repository";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";
import type { WikiNpcPanelViewModel } from "@/viewmodels/wiki-modal/wiki-npc-panel.viewmodel";

export class WikiNpcPanelPresenter {
  public static formatCharismaRange(charismaRequired: NpcCharismaRequiredRaw): string {
    if (charismaRequired.max !== undefined) {
      return `${charismaRequired.min}~${charismaRequired.max}`;
    }

    return `${charismaRequired.min}+`;
  }

  public static getBattleOptions(npcId: string): WikiNpcBattleOptionViewModel[] {
    const npcRaw = NpcRepository.getNpcById(npcId);
    if (npcRaw === undefined) {
      return [];
    }

    const cardOptions = Object.entries(npcRaw.cardBattles ?? {}).map(([battleId, cardBattle]) => {
      return {
        id: `${NpcBattleKindConstant.card}-${battleId}`,
        kind: NpcBattleKindConstant.card,
        battleId,
        charismaMin: cardBattle.charismaRequired.min,
        charismaRangeText: this.formatCharismaRange(cardBattle.charismaRequired),
      };
    });

    const digimonOptions = Object.entries(npcRaw.digimonBattles ?? {}).map(
      ([battleId, digimonBattle]) => {
        return {
          id: `${NpcBattleKindConstant.digimon}-${battleId}`,
          kind: NpcBattleKindConstant.digimon,
          battleId,
          charismaMin: digimonBattle.charismaRequired.min,
          charismaRangeText: this.formatCharismaRange(digimonBattle.charismaRequired),
        };
      },
    );

    return [...cardOptions, ...digimonOptions].sort((firstOption, secondOption) => {
      if (firstOption.charismaMin !== secondOption.charismaMin) {
        return firstOption.charismaMin - secondOption.charismaMin;
      }

      if (firstOption.kind === secondOption.kind) {
        return firstOption.battleId.localeCompare(secondOption.battleId);
      }

      if (firstOption.kind === NpcBattleKindConstant.card) {
        return -1;
      }

      return 1;
    });
  }

  public static getPanelViewModel(npcId: string): WikiNpcPanelViewModel | null {
    const npcRaw = NpcRepository.getNpcById(npcId);
    if (npcRaw === undefined) {
      return null;
    }

    return {
      name: npcRaw.name,
      type: npcRaw.type,
      locationId: npcRaw.locationId,
      battleOptions: this.getBattleOptions(npcId),
    };
  }
}
