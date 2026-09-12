import TamerJson from "@/database/npc/tamer.json";
import type { TamerTable } from "@/repositories/tables/tamer/tamer.table";
import type { TamerRaw } from "@/repositories/tables/raws/tamer/tamer.raw";

export class TamerRepository {
  private static readonly tamerTable = TamerJson as TamerTable;

  public static getTamerById(tamerId: string): TamerRaw | undefined {
    return this.tamerTable[tamerId];
  }

  public static getTamerTable(): TamerTable {
    return this.tamerTable;
  }

  public static getTamerIds(): string[] {
    return Object.keys(this.tamerTable);
  }

  public static getTamerIdByCardBattleId(cardBattleId: number): string | null {
    if (cardBattleId === 0) {
      return null;
    }

    for (const [tamerId, tamerRaw] of Object.entries(this.tamerTable)) {
      for (const cardBattle of Object.values(tamerRaw.cardBattles ?? {})) {
        if (cardBattle.id === cardBattleId) {
          return tamerId;
        }
      }
    }

    return null;
  }
}
