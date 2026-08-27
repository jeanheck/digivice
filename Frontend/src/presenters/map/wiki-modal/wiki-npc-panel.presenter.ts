import { ImageCatalog } from "@/catalogs/image.catalog";
import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { ImportantItems, Npc } from "@/models";
import { NpcRepository } from "@/repositories/npc.repository";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcTrophyRequiredRaw } from "@/repositories/tables/raws/npc/npc-trophy-required.raw";
import { NpcService } from "@/services/npc.service";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";
import type { WikiNpcPanelViewModel } from "@/viewmodels/wiki-modal/wiki-npc-panel.viewmodel";

export class WikiNpcPanelPresenter {
  public static formatCharismaRange(charismaRequired: NpcCharismaRequiredRaw): string {
    if (charismaRequired.max !== undefined) {
      return `${charismaRequired.min}~${charismaRequired.max}`;
    }

    return `${charismaRequired.min}+`;
  }

  private static buildBattleOption(
    kind: NpcBattleKindConstant,
    battleId: string,
    charismaRequired: NpcCharismaRequiredRaw,
    trophyRequired: NpcTrophyRequiredRaw | undefined,
    completed: boolean,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): WikiNpcBattleOptionViewModel {
    const requirementsMet = NpcService.areBattleRequirementsMet(
      charismaRequired,
      trophyRequired,
      partyCharisma,
      importantItems,
    );

    return {
      id: `${kind}-${battleId}`,
      kind,
      battleId,
      charismaMin: charismaRequired.min,
      charismaRangeText: this.formatCharismaRange(charismaRequired),
      completed,
      status: NpcService.getBattleStatus(completed, requirementsMet),
      trophyRequired,
      requirementsMet,
      missingRequirementTooltipKey: NpcService.getMissingRequirementTooltipKey(
        charismaRequired,
        trophyRequired,
        partyCharisma,
        importantItems,
      ),
      showAsukaTrophyEmoji: trophyRequired === "asukaTrophy",
      asukaTrophyOwned: importantItems?.asukaTrophy === true,
    };
  }

  public static getBattleOptions(
    npcId: string,
    journalNpc: Npc | null,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): WikiNpcBattleOptionViewModel[] {
    const npcRaw = NpcRepository.getNpcById(npcId);
    if (npcRaw === undefined) {
      return [];
    }

    const cardOptions = Object.entries(npcRaw.cardBattles ?? {}).map(([battleId, cardBattle]) => {
      return this.buildBattleOption(
        NpcBattleKindConstant.card,
        battleId,
        cardBattle.charismaRequired,
        cardBattle.trophyRequired,
        false,
        partyCharisma,
        importantItems,
      );
    });

    const digimonOptions = Object.entries(npcRaw.digimonBattles ?? {}).map(
      ([battleId, digimonBattle]) => {
        const completed = NpcService.isDigimonBattleCompleted(journalNpc, battleId);

        return this.buildBattleOption(
          NpcBattleKindConstant.digimon,
          battleId,
          digimonBattle.charismaRequired,
          digimonBattle.trophyRequired,
          completed,
          partyCharisma,
          importantItems,
        );
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

  public static getDefaultSelectedBattleOptionId(
    options: WikiNpcBattleOptionViewModel[],
  ): string | null {
    if (options.length === 0) {
      return null;
    }

    if (options.every((option) => option.completed)) {
      const lastOption = options[options.length - 1];
      if (lastOption === undefined) {
        return null;
      }

      return lastOption.id;
    }

    const availableOption = options.find((option) => {
      return option.status === "available";
    });
    if (availableOption !== undefined) {
      return availableOption.id;
    }

    const firstIncompleteOption = options.find((option) => {
      return !option.completed;
    });

    const firstOption = options[0];
    return firstIncompleteOption?.id ?? firstOption?.id ?? null;
  }

  public static getPanelViewModel(
    npcId: string,
    journalNpc: Npc | null,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): WikiNpcPanelViewModel | null {
    const npcRaw = NpcRepository.getNpcById(npcId);
    if (npcRaw === undefined) {
      return null;
    }

    return {
      name: npcRaw.name,
      type: npcRaw.type,
      locationId: npcRaw.locationId,
      imageUrl: ImageCatalog.getNpcImageUrl(npcRaw.name),
      battleOptions: this.getBattleOptions(npcId, journalNpc, partyCharisma, importantItems),
    };
  }
}
