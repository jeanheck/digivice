import DesertAreasJson from "@/database/mobius-desert/mobius-desert-areas.json";
import type { DesertAreasRaw } from "./tables/raws/desert/desert-areas.raw";
import type { DesertAreasTable } from "./tables/desert/desert-areas.table";

export class MobiusDesertAreasRepository {
  private static readonly desertAreasTable = DesertAreasJson as DesertAreasTable;

  public static getAll(): DesertAreasRaw {
    return this.desertAreasTable;
  }
}
