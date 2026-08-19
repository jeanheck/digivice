import BoosterJson from "@/database/tcg/booster.json";
import type { BoostersTable } from "@/repositories/tables/tcg/boosters.table";
import type { BoosterRaw } from "@/repositories/tables/raws/tcg/booster.raw";

export class BoosterRepository {
  private static readonly boostersTable = BoosterJson as BoostersTable;

  public static getById(boosterId: number): BoosterRaw | undefined {
    return this.boostersTable[String(boosterId)];
  }
}
