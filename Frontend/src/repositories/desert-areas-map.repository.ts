import DesertAreasMapJson from "@/database/desert/desert-areas-map.json";
import type { DesertAreaMapCellRaw } from "./tables/raws/desert/desert-area-map-cell.raw";
import type { DesertAreasMapTable } from "./tables/desert/desert-areas-map.table";

export class DesertAreasMapRepository {
  private static readonly desertAreasMapTable = DesertAreasMapJson as DesertAreasMapTable;

  public static getCell(locationId: string, routeTypeKey: string): DesertAreaMapCellRaw | null {
    return this.desertAreasMapTable[locationId]?.[routeTypeKey] ?? null;
  }
}
