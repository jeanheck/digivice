import DesertAreasJson from "@/database/mobius-desert/mobius-desert-areas.json";
import type { MobiusDesertAreasRaw } from "./tables/raws/mobius-desert/mobius-desert-areas.raw";
import type { MobiusDesertAreasTable } from "./tables/desert/mobius-desert-areas.table";

export class MobiusDesertAreasRepository {
  private static readonly desertAreasTable = DesertAreasJson as MobiusDesertAreasTable;

  public static getAll(): MobiusDesertAreasRaw {
    return this.desertAreasTable;
  }
}
