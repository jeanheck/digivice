import { ImageCatalog } from "@/catalogs/image.catalog";
import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { ImportantItems, Npc } from "@/models";
import { NpcRepository } from "@/repositories/npc.repository";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcTrophyRequiredRaw } from "@/repositories/tables/raws/npc/npc-trophy-required.raw";
import {
  NpcService,
  UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY,
} from "@/services/npc.service";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";
import type { WikiNpcPanelViewModel } from "@/viewmodels/wiki-modal/wiki-npc-panel.viewmodel";

export class WikiNpcPanelPresenter {
  public static formatCharismaRange(charismaRequired: NpcCharismaRequiredRaw): string {
    if (charismaRequired.max !== undefined) {
      return `${charismaRequired.min}~${charismaRequired.max}`;
    }

    return `${charismaRequired.min}+`;
  }

  private static buildBattleOptionBase(
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
      status: NpcService.getBattleStatus(completed, false),
      trophyRequired,
      requirementsMet,
      isActive: false,
      isSuperseded: false,
      missingRequirementTooltipKey: NpcService.getMissingRequirementTooltipKey(
        charismaRequired,
        trophyRequired,
        partyCharisma,
        importantItems,
      ),
      supersededTooltipKey: null,
      battleTooltipKey: null,
      showAsukaTrophyEmoji: trophyRequired === "asukaTrophy",
      asukaTrophyOwned: importantItems?.asukaTrophy === true,
    };
  }

  private static finalizeBattleTooltip(option: WikiNpcBattleOptionViewModel): void {
    option.battleTooltipKey = NpcService.getBattleTooltipKey({
      status: option.status,
      isSuperseded: option.isSuperseded,
      missingRequirementTooltipKey: option.missingRequirementTooltipKey,
    });
  }

  private static applyDigimonActivation(option: WikiNpcBattleOptionViewModel): void {
    const isActive = option.requirementsMet && !option.completed;
    option.isActive = isActive;
    option.isSuperseded = false;
    option.supersededTooltipKey = null;
    option.status = NpcService.getBattleStatus(option.completed, isActive);
    this.finalizeBattleTooltip(option);
  }

  private static applyCardActivation(
    option: WikiNpcBattleOptionViewModel,
    activeCardBattleIds: Set<string>,
    importantItems: ImportantItems | null | undefined,
  ): void {
    const isActive = activeCardBattleIds.has(option.battleId);
    const activeBattleId = [...activeCardBattleIds][0];
    const isLowerTierThanActive =
      activeBattleId !== undefined && option.battleId.localeCompare(activeBattleId) < 0;
    const isSuperseded =
      !isActive &&
      (option.requirementsMet ||
        (importantItems?.asukaTrophy === true && isLowerTierThanActive));

    option.isActive = isActive;
    option.isSuperseded = isSuperseded;
    option.supersededTooltipKey = isSuperseded ? UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY : null;
    option.status = NpcService.getBattleStatus(false, isActive);
    this.finalizeBattleTooltip(option);
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

    const activeCardBattleIds = NpcService.resolveActiveCardBattleIds(
      npcRaw.cardBattles,
      partyCharisma,
      importantItems,
    );

    const cardOptions = Object.entries(npcRaw.cardBattles ?? {}).map(([battleId, cardBattle]) => {
      const option = this.buildBattleOptionBase(
        NpcBattleKindConstant.card,
        battleId,
        cardBattle.charismaRequired,
        cardBattle.trophyRequired,
        false,
        partyCharisma,
        importantItems,
      );
      this.applyCardActivation(option, activeCardBattleIds, importantItems);

      return option;
    });

    const digimonOptions = Object.entries(npcRaw.digimonBattles ?? {}).map(
      ([battleId, digimonBattle]) => {
        const completed = NpcService.isDigimonBattleCompleted(journalNpc, battleId);
        const option = this.buildBattleOptionBase(
          NpcBattleKindConstant.digimon,
          battleId,
          digimonBattle.charismaRequired,
          digimonBattle.trophyRequired,
          completed,
          partyCharisma,
          importantItems,
        );
        this.applyDigimonActivation(option);

        return option;
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

    const digimonOptions = options
      .filter((option) => {
        return option.kind === NpcBattleKindConstant.digimon;
      })
      .sort((firstOption, secondOption) => {
        return firstOption.battleId.localeCompare(secondOption.battleId);
      });

    const firstAvailableDigimonOption = digimonOptions.find((option) => {
      return option.status === "available";
    });
    if (firstAvailableDigimonOption !== undefined) {
      return firstAvailableDigimonOption.id;
    }

    const cardOptions = options
      .filter((option) => {
        return option.kind === NpcBattleKindConstant.card;
      })
      .sort((firstOption, secondOption) => {
        return firstOption.battleId.localeCompare(secondOption.battleId);
      });

    const availableCardOptions = cardOptions.filter((option) => {
      return option.status === "available";
    });
    if (availableCardOptions.length > 0) {
      return availableCardOptions[availableCardOptions.length - 1]?.id ?? null;
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
