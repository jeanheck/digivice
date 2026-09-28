import DuelIslandJson from "@/database/npc/duel-island.json";
import type { DuelIslandTable } from "@/repositories/tables/duel-island/duel-island.table";
import type { DuelIslandRaw } from "@/repositories/tables/raws/duel-island/duel-island.raw";

export class DuelIslandRepository {
  private static readonly duelIslandTable = DuelIslandJson as DuelIslandTable;
  private static readonly duelIslandIdsByBoosterId = this.buildDuelIslandIdsByBoosterId();

  private static buildDuelIslandIdsByBoosterId(): Map<number, string[]> {
    const duelIslandIdsByBoosterId = new Map<number, string[]>();
    for (const [duelIslandId, duelIslandRaw] of Object.entries(this.duelIslandTable)) {
      const boosterIds = new Set(
        Object.values(duelIslandRaw.cardBattles ?? {}).map((cardBattle) => {
          return cardBattle.boosterId;
        }),
      );

      for (const boosterId of boosterIds) {
        const duelIslandIds = duelIslandIdsByBoosterId.get(boosterId) ?? [];
        duelIslandIds.push(duelIslandId);
        duelIslandIdsByBoosterId.set(boosterId, duelIslandIds);
      }
    }

    return duelIslandIdsByBoosterId;
  }

  public static getDuelIslandById(duelIslandId: string): DuelIslandRaw | undefined {
    return this.duelIslandTable[duelIslandId];
  }

  public static getDuelIslandTable(): DuelIslandTable {
    return this.duelIslandTable;
  }

  public static getDuelIslandIds(): string[] {
    return Object.keys(this.duelIslandTable);
  }

  public static getDuelIslandsWhoDropByBooster(boosterId: number): string[] {
    return this.duelIslandIdsByBoosterId.get(boosterId) ?? [];
  }

  public static getDuelIslandIdByCardBattleId(cardBattleId: number | null): string | null {
    if (cardBattleId === null) {
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
