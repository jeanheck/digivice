import BoosterJson from "@/database/tcg/booster.json";
import type { BoosterTable } from "@/repositories/tables/tcg/booster.table";

export class BoosterRepository {
  private static readonly boosterTable = BoosterJson as BoosterTable;
  private static readonly boosterIdsByCardId = this.buildBoosterIdsByCardId();

  private static buildBoosterIdsByCardId(): Map<number, number[]> {
    const boosterIdsByCardId = new Map<number, number[]>();
    for (const [boosterId, cardIds] of Object.entries(this.boosterTable)) {
      for (const cardId of cardIds) {
        const boosterIds = boosterIdsByCardId.get(cardId) ?? [];
        boosterIds.push(Number(boosterId));
        boosterIdsByCardId.set(cardId, boosterIds);
      }
    }

    return boosterIdsByCardId;
  }

  public static getIds(): string[] {
    return Object.keys(this.boosterTable);
  }

  public static getCardIdsByBoosterId(boosterId: number): number[] {
    return this.boosterTable[String(boosterId)] ?? [];
  }

  public static getBoosterIdsByCardId(cardId: number): number[] {
    return this.boosterIdsByCardId.get(cardId) ?? [];
  }
}
