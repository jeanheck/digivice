import DesertAreasJson from "@/database/desert/desert-areas.json";
import type { DesertAreasRaw } from "./tables/raws/desert/desert-areas.raw";
import type { DesertAreasTable } from "./tables/desert/desert-areas.table";

export class DesertAreasRepository {
  private static readonly desertAreasTable = DesertAreasJson as DesertAreasTable;

  public static getAll(): DesertAreasRaw {
    return this.desertAreasTable;
  }
}
