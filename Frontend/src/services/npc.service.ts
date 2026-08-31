import type { ImportantItems, Npc } from "@/models";
import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { TamerCardBattleRaw } from "@/repositories/tables/raws/tamer/tamer-card-battle.raw";
import type { TamerCharismaRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-charisma-required.raw";
import type { TamerRaw } from "@/repositories/tables/raws/tamer/tamer.raw";
import type { TamerTrophyRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-trophy-required.raw";

export type NpcBattleStatus = "completed" | "available" | "missingRequirements";

export const MISSING_CHARISMA_TOOLTIP_KEY = "npc.battle.requirement.missingCharisma";
export const MISSING_ASUKA_TROPHY_TOOLTIP_KEY = "npc.battle.requirement.missingAsukaTrophy";
export const MISSING_CHARISMA_AND_ASUKA_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.missingCharismaAndAsukaTrophy";
export const UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.unavailableAfterAsukaTrophy";
export const AVAILABLE_BATTLE_TOOLTIP_KEY = "npc.battle.requirement.available";
export const ALREADY_WON_BATTLE_TOOLTIP_KEY = "npc.battle.requirement.alreadyWon";

export class NpcService {
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

    return false;
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
      return MISSING_CHARISMA_AND_ASUKA_TROPHY_TOOLTIP_KEY;
    }

    if (!charismaMet) {
      return MISSING_CHARISMA_TOOLTIP_KEY;
    }

    return MISSING_ASUKA_TROPHY_TOOLTIP_KEY;
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

    const battle = journalNpc.digimonBattles.find((entry) => {
      return entry.id === battleId;
    });

    return battle?.completed ?? false;
  }

  public static getBattleTooltipKey(params: {
    status: NpcBattleStatus;
    isSuperseded: boolean;
    missingRequirementTooltipKey: string | null;
  }): string | null {
    if (params.status === "completed") {
      return ALREADY_WON_BATTLE_TOOLTIP_KEY;
    }

    if (params.status === "available") {
      return AVAILABLE_BATTLE_TOOLTIP_KEY;
    }

    if (params.isSuperseded) {
      return UNAVAILABLE_AFTER_ASUKA_TROPHY_TOOLTIP_KEY;
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
    tamer: TamerRaw,
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
    tamer: TamerRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    return (
      this.getAvailableBattleKind(tamer, journalNpc, partyCharisma, importantItems) !== null
    );
  }
}
