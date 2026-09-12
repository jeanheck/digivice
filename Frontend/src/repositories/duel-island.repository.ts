import DuelIslandJson from "@/database/npc/duel-island.json";
import type { DuelIslandTable } from "@/repositories/tables/duel-island/duel-island.table";
import type { DuelIslandRaw } from "@/repositories/tables/raws/duel-island/duel-island.raw";

export class DuelIslandRepository {
  private static readonly duelIslandTable = DuelIslandJson as DuelIslandTable;

  public static getDuelIslandById(duelIslandId: string): DuelIslandRaw | undefined {
    return this.duelIslandTable[duelIslandId];
  }

  public static getDuelIslandTable(): DuelIslandTable {
    return this.duelIslandTable;
  }

  public static getDuelIslandIds(): string[] {
    return Object.keys(this.duelIslandTable);
  }

  public static getDuelIslandIdByCardBattleId(cardBattleId: number): string | null {
    if (cardBattleId === 0) {
      return null;
    }

    for (const [duelIslandId, duelIslandRaw] of Object.entries(this.duelIslandTable)) {
      for (const cardBattle of Object.values(duelIslandRaw.cardBattles ?? {})) {
        if (cardBattle.id === cardBattleId) {
          return duelIslandId;
        }
      }
    }

    return null;
  }
}
