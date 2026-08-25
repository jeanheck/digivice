import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { Npc } from "@/models";
import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";

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

  public static isBattleCompleted(
    journalNpc: Npc | null | undefined,
    kind: NpcBattleKindConstant,
    battleId: string,
  ): boolean {
    if (journalNpc === null || journalNpc === undefined) {
      return false;
    }

    const battles =
      kind === NpcBattleKindConstant.card
        ? journalNpc.cardBattles
        : journalNpc.digimonBattles;

    const battle = battles.find((entry) => {
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

  public static hasAvailableBattle(
    npc: NpcRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
  ): boolean {
    const hasAvailableCardBattle = Object.entries(npc.cardBattles ?? {}).some(
      ([battleId, cardBattle]) => {
        const completed = this.isBattleCompleted(
          journalNpc,
          NpcBattleKindConstant.card,
          battleId,
        );

        return this.isBattleAvailable(
          cardBattle.charismaRequired,
          completed,
          partyCharisma,
        );
      },
    );
    if (hasAvailableCardBattle) {
      return true;
    }

    return Object.entries(npc.digimonBattles ?? {}).some(([battleId, digimonBattle]) => {
      const completed = this.isBattleCompleted(
        journalNpc,
        NpcBattleKindConstant.digimon,
        battleId,
      );

      return this.isBattleAvailable(
        digimonBattle.charismaRequired,
        completed,
        partyCharisma,
      );
    });
  }
}
