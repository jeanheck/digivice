import {
  NpcBattleKindConstant,
  STORY_NPC_DIGIMON_BATTLE_ID,
} from "@/constants/npc-battle-kind.constant";
import type { ImportantItems, Npc } from "@/models";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import type { TamerCharismaRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-charisma-required.raw";
import type { TamerTrophyRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-trophy-required.raw";
import {
  NpcService,
} from "@/services/npc.service";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";
import type { WikiNpcPanelViewModel } from "@/viewmodels/wiki-modal/wiki-npc-panel.viewmodel";

export class WikiNpcPanelPresenter {
  public static formatCharismaRange(charismaRequired: TamerCharismaRequiredRaw): string {
    if (charismaRequired.max !== undefined) {
      return `${charismaRequired.min}~${charismaRequired.max}`;
    }

    return `${charismaRequired.min}+`;
  }

  private static buildBattleOptionBase(
    kind: NpcBattleKindConstant,
    battleId: string,
    charismaRequired: TamerCharismaRequiredRaw,
    trophyRequired: TamerTrophyRequiredRaw | undefined,
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
      showTrophyEmoji: trophyRequired !== undefined,
      trophyOwned: NpcService.isTrophyOwned(trophyRequired, importantItems),
    };
  }

  private static finalizeBattleTooltip(option: WikiNpcBattleOptionViewModel): void {
    option.battleTooltipKey = NpcService.getBattleTooltipKey({
      status: option.status,
      isSuperseded: option.isSuperseded,
      missingRequirementTooltipKey: option.missingRequirementTooltipKey,
      supersededTooltipKey: option.supersededTooltipKey,
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
    activeTrophyRequired: TamerTrophyRequiredRaw | undefined,
  ): void {
    const isActive = activeCardBattleIds.has(option.battleId);
    const activeBattleId = [...activeCardBattleIds][0];
    const isLowerTierThanActive =
      activeBattleId !== undefined && option.battleId.localeCompare(activeBattleId) < 0;
    const hasRematchTrophy =
      importantItems?.asukaTrophy === true || importantItems?.sunTrophy === true;
    const isSuperseded =
      !isActive &&
      (option.requirementsMet || (hasRematchTrophy && isLowerTierThanActive));

    option.isActive = isActive;
    option.isSuperseded = isSuperseded;
    option.supersededTooltipKey = isSuperseded
      ? NpcService.getUnavailableAfterTrophyTooltipKey(activeTrophyRequired)
      : null;
    option.status = NpcService.getBattleStatus(false, isActive);
    this.finalizeBattleTooltip(option);
  }

  private static buildStoryNpcBattleOption(
    journalNpc: Npc | null,
    battleId: string,
  ): WikiNpcBattleOptionViewModel {
    const completed = NpcService.isDigimonBattleCompleted(journalNpc, battleId);
    const option: WikiNpcBattleOptionViewModel = {
      id: `${NpcBattleKindConstant.digimon}-${battleId}`,
      kind: NpcBattleKindConstant.digimon,
      battleId,
      charismaMin: 0,
      charismaRangeText: "",
      completed,
      status: NpcService.getBattleStatus(completed, false),
      requirementsMet: true,
      isActive: false,
      isSuperseded: false,
      missingRequirementTooltipKey: null,
      supersededTooltipKey: null,
      battleTooltipKey: null,
      showTrophyEmoji: false,
      trophyOwned: false,
    };
    this.applyDigimonActivation(option);

    return option;
  }

  public static getBattleOptions(
    npcId: string,
    journalNpc: Npc | null,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): WikiNpcBattleOptionViewModel[] {
    const opponent = NpcBattleOpponentHelper.resolveById(npcId);
    if (opponent === undefined) {
      return [];
    }

    if (opponent.source === "npc") {
      return [this.buildStoryNpcBattleOption(journalNpc, STORY_NPC_DIGIMON_BATTLE_ID)];
    }

    const opponentRaw = opponent.raw;

    const activeCardBattleIds = NpcService.resolveActiveCardBattleIds(
      opponentRaw.cardBattles,
      partyCharisma,
      importantItems,
    );
    const activeBattleId = [...activeCardBattleIds][0];
    const activeTrophyRequired =
      activeBattleId !== undefined
        ? opponentRaw.cardBattles?.[activeBattleId]?.trophyRequired
        : undefined;

    const cardOptions = Object.entries(opponentRaw.cardBattles ?? {}).map(([battleId, cardBattle]) => {
      const option = this.buildBattleOptionBase(
        NpcBattleKindConstant.card,
        battleId,
        cardBattle.charismaRequired,
        cardBattle.trophyRequired,
        false,
        partyCharisma,
        importantItems,
      );
      this.applyCardActivation(option, activeCardBattleIds, importantItems, activeTrophyRequired);

      return option;
    });

    const digimonOptions = Object.entries(opponentRaw.digimonBattles ?? {}).map(
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
    const opponent = NpcBattleOpponentHelper.resolveById(npcId);
    const nameKey = NpcBattleOpponentHelper.getNameKey(npcId);
    const searchKind = NpcBattleOpponentHelper.getSearchKind(npcId);
    if (opponent === undefined || nameKey === null || searchKind === null) {
      return null;
    }

    return {
      nameKey,
      searchKind,
      locationId: opponent.raw.locationId,
      imageUrl: NpcBattleOpponentHelper.getImageUrl(npcId),
      battleOptions: this.getBattleOptions(npcId, journalNpc, partyCharisma, importantItems),
    };
  }
}
