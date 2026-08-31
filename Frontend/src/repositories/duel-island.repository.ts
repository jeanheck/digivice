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

  public static getDuelIslandIdByOpponentId(opponentId: number): string | null {
    if (opponentId === 0) {
      return null;
    }

    for (const [duelIslandId, duelIslandRaw] of Object.entries(this.duelIslandTable)) {
      if (duelIslandRaw.opponentId === opponentId) {
        return duelIslandId;
      }
    }

    return null;
  }
}
