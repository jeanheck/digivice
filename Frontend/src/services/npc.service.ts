import type { ImportantItems, Npc } from "@/models";
import type { NpcCardBattleRaw } from "@/repositories/tables/raws/npc/npc-card-battle.raw";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";
import type { NpcTrophyRequiredRaw } from "@/repositories/tables/raws/npc/npc-trophy-required.raw";

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
    charismaRequired: NpcCharismaRequiredRaw,
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
    trophyRequired: NpcTrophyRequiredRaw | undefined,
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
    charismaRequired: NpcCharismaRequiredRaw,
    trophyRequired: NpcTrophyRequiredRaw | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    if (!this.isCharismaInRange(partyCharisma, charismaRequired)) {
      return false;
    }

    return this.isTrophyRequirementMet(trophyRequired, importantItems);
  }

  public static getMissingRequirementTooltipKey(
    charismaRequired: NpcCharismaRequiredRaw,
    trophyRequired: NpcTrophyRequiredRaw | undefined,
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
    cardBattles: Record<string, NpcCardBattleRaw> | undefined,
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

  public static hasAvailableBattle(
    npc: NpcRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems | null | undefined,
  ): boolean {
    const activeCardBattleIds = this.resolveActiveCardBattleIds(
      npc.cardBattles,
      partyCharisma,
      importantItems,
    );
    if (activeCardBattleIds.size > 0) {
      return true;
    }

    return Object.entries(npc.digimonBattles ?? {}).some(([battleId, digimonBattle]) => {
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
    });
  }
}
