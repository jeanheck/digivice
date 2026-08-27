import type { Npc } from "@/models";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";

export type NpcBattleStatus = "completed" | "available" | "missingRequirements";

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

  public static isBattleAvailable(
    charismaRequired: NpcCharismaRequiredRaw,
    completed: boolean,
    partyCharisma: number,
  ): boolean {
    if (completed) {
      return false;
    }

    return this.isCharismaInRange(partyCharisma, charismaRequired);
  }

  public static getBattleStatus(
    completed: boolean,
    charismaRequired: NpcCharismaRequiredRaw,
    partyCharisma: number,
  ): NpcBattleStatus {
    if (completed) {
      return "completed";
    }

    if (this.isBattleAvailable(charismaRequired, completed, partyCharisma)) {
      return "available";
    }

    return "missingRequirements";
  }

  public static hasAvailableBattle(
    npc: NpcRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
  ): boolean {
    const hasAvailableCardBattle = Object.values(npc.cardBattles ?? {}).some((cardBattle) => {
      return this.isCharismaInRange(partyCharisma, cardBattle.charismaRequired);
    });
    if (hasAvailableCardBattle) {
      return true;
    }

    return Object.entries(npc.digimonBattles ?? {}).some(([battleId, digimonBattle]) => {
      const completed = this.isDigimonBattleCompleted(journalNpc, battleId);

      return this.isBattleAvailable(
        digimonBattle.charismaRequired,
        completed,
        partyCharisma,
      );
    });
  }
}
