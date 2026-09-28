import TamerJson from "@/database/npc/tamer.json";
import type { TamerTable } from "@/repositories/tables/tamer/tamer.table";
import type { TamerRaw } from "@/repositories/tables/raws/tamer/tamer.raw";

export class TamerRepository {
  private static readonly tamerTable = TamerJson as TamerTable;
  private static readonly tamerIdsByBoosterId = this.buildTamerIdsByBoosterId();

  private static buildTamerIdsByBoosterId(): Map<number, string[]> {
    const tamerIdsByBoosterId = new Map<number, string[]>();
    for (const [tamerId, tamerRaw] of Object.entries(this.tamerTable)) {
      const boosterIds = new Set(
        Object.values(tamerRaw.cardBattles ?? {}).map((cardBattle) => {
          return cardBattle.boosterId;
        }),
      );

      for (const boosterId of boosterIds) {
        const tamerIds = tamerIdsByBoosterId.get(boosterId) ?? [];
        tamerIds.push(tamerId);
        tamerIdsByBoosterId.set(boosterId, tamerIds);
      }
    }

    return tamerIdsByBoosterId;
  }

  public static getTamerById(tamerId: string): TamerRaw | undefined {
    return this.tamerTable[tamerId];
  }

  public static getTamerTable(): TamerTable {
    return this.tamerTable;
  }

  public static getTamerIds(): string[] {
    return Object.keys(this.tamerTable);
  }

  public static getTamersWhoDropByBooster(boosterId: number): string[] {
    return this.tamerIdsByBoosterId.get(boosterId) ?? [];
  }

  public static getTamerIdByCardBattleId(cardBattleId: number | null): string | null {
    if (cardBattleId === null) {
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
