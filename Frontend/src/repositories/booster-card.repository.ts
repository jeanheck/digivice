import BoosterCardJson from "@/database/tcg/booster-card.json";
import type { BoosterCardTable } from "@/repositories/tables/tcg/booster-card.table";

export class BoosterCardRepository {
  private static readonly boosterCardTable = BoosterCardJson as BoosterCardTable;
  private static readonly boosterIdsByCardId = this.buildBoosterIdsByCardId();

  private static buildBoosterIdsByCardId(): Map<number, number[]> {
    const boosterIdsByCardId = new Map<number, number[]>();
    for (const [boosterId, cardIds] of Object.entries(this.boosterCardTable)) {
      for (const cardId of cardIds) {
        const boosterIds = boosterIdsByCardId.get(cardId) ?? [];
        boosterIds.push(Number(boosterId));
        boosterIdsByCardId.set(cardId, boosterIds);
      }
    }

    return boosterIdsByCardId;
  }

  public static getCardIdsByBoosterId(boosterId: number): number[] {
    return this.boosterCardTable[String(boosterId)] ?? [];
  }
  public static getBoosterIdsByCardId(cardId: number): number[] {
    return this.boosterIdsByCardId.get(cardId) ?? [];
  }
}
