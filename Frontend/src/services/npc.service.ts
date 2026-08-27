import type { ImportantItems, Npc } from "@/models";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";
import type { NpcTrophyRequiredRaw } from "@/repositories/tables/raws/npc/npc-trophy-required.raw";

export type NpcBattleStatus = "completed" | "available" | "missingRequirements";

const MISSING_CHARISMA_TOOLTIP_KEY = "npc.battle.requirement.missingCharisma";
const MISSING_ASUKA_TROPHY_TOOLTIP_KEY = "npc.battle.requirement.missingAsukaTrophy";
const MISSING_CHARISMA_AND_ASUKA_TROPHY_TOOLTIP_KEY =
  "npc.battle.requirement.missingCharismaAndAsukaTrophy";

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

  public static getBattleStatus(
    completed: boolean,
    requirementsMet: boolean,
  ): NpcBattleStatus {
    if (completed) {
      return "completed";
    }

    if (requirementsMet) {
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
    const hasAvailableCardBattle = Object.values(npc.cardBattles ?? {}).some((cardBattle) => {
      return this.areBattleRequirementsMet(
        cardBattle.charismaRequired,
        cardBattle.trophyRequired,
        partyCharisma,
        importantItems,
      );
    });
    if (hasAvailableCardBattle) {
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
