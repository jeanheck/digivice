import BoosterJson from "@/database/tcg/booster.json";
import type { BoosterTable } from "@/repositories/tables/tcg/booster.table";
import type { BoosterRaw } from "@/repositories/tables/raws/tcg/booster.raw";

export class BoosterRepository {
  private static readonly boosterTable = BoosterJson as BoosterTable;

  public static getById(boosterId: number): BoosterRaw | undefined {
    return this.boosterTable[String(boosterId)];
  }
}
