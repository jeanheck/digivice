import type { ImportantItems, Npc } from "@/models";
import { NpcBattleKindConstant, STORY_NPC_DIGIMON_BATTLE_ID } from "@/constants/npc-battle-kind.constant";
import type {
  NpcBattleOpponent,
  NpcBattleOpponentRaw,
} from "@/presenters/helper/npc-battle-opponent.helper";
import type { TamerCardBattleRaw } from "@/repositories/tables/raws/tamer/tamer-card-battle.raw";
import type { TamerCharismaRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-charisma-required.raw";
import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { TamerTrophyRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-trophy-required.raw";

export type NpcBattleStatus = "completed" | "available" | "missingRequirements";

export const MISSING_CHARISMA_TOOLTIP_KEY = "npc.battle.requirement.missingCharisma";
export const MISSING_ASUKA_TROPHY_TOOLTIP_KEY = "npc.battle.requirement.missingAsukaTrophy";
export const MISSING_SUN_TROPHY_TOOLTIP_KEY = "npc.battle.requirement.missingSunTrophy";
export const MISSING_CHARISMA_AND_ASUKA_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.missingCharismaAndAsukaTrophy";
export const MISSING_CHARISMA_AND_SUN_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.missingCharismaAndSunTrophy";
export const UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.unavailableAfterAsukaTrophy";
export const UNAVAILABLE_AFTER_SUN_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.unavailableAfterSunTrophy";
export const AVAILABLE_BATTLE_TOOLTIP_KEY = "npc.battle.requirement.available";
export const ALREADY_WON_BATTLE_TOOLTIP_KEY = "npc.battle.requirement.alreadyWon";

export class NpcService {
  public static isVisibleOnMapByMainQuestStep(
    lastCompletedMainQuestStep: number,
    mainQuestStepDone?: NpcMainQuestStepDoneRaw,
  ): boolean {
    if (mainQuestStepDone === undefined) {
      return true;
    }

    if (lastCompletedMainQuestStep < mainQuestStepDone.min) {
      return false;
    }

    if (
      mainQuestStepDone.max !== undefined &&
      lastCompletedMainQuestStep >= mainQuestStepDone.max
    ) {
      return false;
    }

    return true;
  }

  public static isCharismaInRange(
    partyCharisma: number,
    charismaRequired: TamerCharismaRequiredRaw,
  ): boolean {
    if (partyCharisma < charismaRequired.min) {
      return false;
    }

    if (charismaRequired.max !== undefined && partyCharisma > charismaRequired.max) {
      return false;
    }

    return true;
  }

  public static isTrophyRequirementMet(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    if (trophyRequired === undefined) {
      return true;
    }

    if (trophyRequired === "asukaTrophy") {
      return importantItems?.asukaTrophy === true;
    }

    if (trophyRequired === "sunTrophy") {
      return importantItems?.sunTrophy === true;
    }

    return false;
  }

  private static getMissingTrophyTooltipKey(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
  ): string | null {
    if (trophyRequired === "asukaTrophy") {
      return MISSING_ASUKA_TROPHY_TOOLTIP_KEY;
    }

    if (trophyRequired === "sunTrophy") {
      return MISSING_SUN_TROPHY_TOOLTIP_KEY;
    }

    return null;
  }

  private static getMissingCharismaAndTrophyTooltipKey(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
  ): string {
    if (trophyRequired === "sunTrophy") {
      return MISSING_CHARISMA_AND_SUN_TROPHY_TOOLTIP_KEY;
    }

    return MISSING_CHARISMA_AND_ASUKA_TROPHY_TOOLTIP_KEY;
  }

  public static getUnavailableAfterTrophyTooltipKey(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
  ): string {
    if (trophyRequired === "sunTrophy") {
      return UNAVAILABLE_AFTER_SUN_TROPHY_TOOLTIP_KEY;
    }

    return UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY;
  }

  public static isTrophyOwned(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    return this.isTrophyRequirementMet(trophyRequired, importantItems);
  }

  public static areBattleRequirementsMet(
    charismaRequired: TamerCharismaRequiredRaw,
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    if (!this.isCharismaInRange(partyCharisma, charismaRequired)) {
      return false;
    }

    return this.isTrophyRequirementMet(trophyRequired, importantItems);
  }

  public static getMissingRequirementTooltipKey(
    charismaRequired: TamerCharismaRequiredRaw,
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): string | null {
    const charismaMet = this.isCharismaInRange(partyCharisma, charismaRequired);
    const trophyMet = this.isTrophyRequirementMet(trophyRequired, importantItems);

    if (charismaMet && trophyMet) {
      return null;
    }

    if (!charismaMet && !trophyMet) {
      return this.getMissingCharismaAndTrophyTooltipKey(trophyRequired);
    }

    if (!charismaMet) {
      return MISSING_CHARISMA_TOOLTIP_KEY;
    }

    return this.getMissingTrophyTooltipKey(trophyRequired);
  }

  public static resolveActiveCardBattleIds(
    cardBattles: Record<string, TamerCardBattleRaw> | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): Set<string> {
    const sortedCardBattles = Object.entries(cardBattles ?? {}).sort(
      ([firstBattleId], [secondBattleId]) => {
        return firstBattleId.localeCompare(secondBattleId);
      },
    );

    let activeBattleId: string | null = null;

    for (const [battleId, cardBattle] of sortedCardBattles) {
      if (
        this.areBattleRequirementsMet(
          cardBattle.charismaRequired,
          cardBattle.trophyRequired,
          partyCharisma,
          importantItems,
        )
      ) {
        activeBattleId = battleId;
      }
    }

    if (activeBattleId === null) {
      return new Set();
    }

    return new Set([activeBattleId]);
  }

  public static isDigimonBattleCompleted(
    journalNpc: Npc | null | undefined,
    battleId: string,
  ): boolean {
    if (journalNpc === null || journalNpc === undefined) {
      return false;
    }

    const battle = journalNpc.battles.find((entry) => {
      return entry.id === battleId;
    });

    return battle?.completed ?? false;
  }

  public static getBattleTooltipKey(params: {
    status: NpcBattleStatus;
    isSuperseded: boolean;
    missingRequirementTooltipKey: string | null;
    supersededTooltipKey: string | null;
  }): string | null {
    if (params.status === "completed") {
      return ALREADY_WON_BATTLE_TOOLTIP_KEY;
    }

    if (params.status === "available") {
      return AVAILABLE_BATTLE_TOOLTIP_KEY;
    }

    if (params.isSuperseded) {
      return params.supersededTooltipKey ?? UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY;
    }

    return params.missingRequirementTooltipKey;
  }

  public static getBattleStatus(completed: boolean, isActive: boolean): NpcBattleStatus {
    if (completed) {
      return "completed";
    }

    if (isActive) {
      return "available";
    }

    return "missingRequirements";
  }

  public static getAvailableBattleKind(
    opponent: NpcBattleOpponentRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): NpcBattleKindConstant | null {
    return this.getAvailableBattleKindFromTamerOrDuelIsland(
      opponent,
      journalNpc,
      partyCharisma,
      importantItems,
    );
  }

  public static getAvailableBattleKindForOpponent(
    opponent: NpcBattleOpponent,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): NpcBattleKindConstant | null {
    if (opponent.source === "npc") {
      const completed = this.isDigimonBattleCompleted(journalNpc, STORY_NPC_DIGIMON_BATTLE_ID);
      if (completed) {
        return null;
      }

      return NpcBattleKindConstant.digimon;
    }

    return this.getAvailableBattleKindFromTamerOrDuelIsland(
      opponent.raw,
      journalNpc,
      partyCharisma,
      importantItems,
    );
  }

  private static getAvailableBattleKindFromTamerOrDuelIsland(
    tamer: NpcBattleOpponentRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): NpcBattleKindConstant | null {
    const hasAvailableDigimonBattle = Object.entries(tamer.digimonBattles ?? {}).some(
      ([battleId, digimonBattle]) => {
        const completed = this.isDigimonBattleCompleted(journalNpc, battleId);
        if (completed) {
          return false;
        }

        return this.areBattleRequirementsMet(
          digimonBattle.charismaRequired,
          digimonBattle.trophyRequired,
          partyCharisma,
          importantItems,
        );
      },
    );

    if (hasAvailableDigimonBattle) {
      return NpcBattleKindConstant.digimon;
    }

    const activeCardBattleIds = this.resolveActiveCardBattleIds(
      tamer.cardBattles,
      partyCharisma,
      importantItems,
    );
    if (activeCardBattleIds.size > 0) {
      return NpcBattleKindConstant.card;
    }

    return null;
  }

  public static hasAvailableBattle(
    opponent: NpcBattleOpponent,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    return (
      this.getAvailableBattleKindForOpponent(
        opponent,
        journalNpc,
        partyCharisma,
        importantItems,
      ) !== null
    );
  }
}
