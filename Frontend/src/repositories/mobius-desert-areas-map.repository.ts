import DesertAreasMapJson from "@/database/mobius-desert/mobius-desert-areas-map.json";
import type { DesertAreaMapCellRaw } from "./tables/raws/desert/desert-area-map-cell.raw";
import type { DesertAreasMapTable } from "./tables/desert/desert-areas-map.table";

export class MobiusDesertAreasMapRepository {
  private static readonly desertAreasMapTable = DesertAreasMapJson as DesertAreasMapTable;

  public static findByLabel(label: string): { locationId: string; cell: DesertAreaMapCellRaw } | null {
    for (const [locationId, cells] of Object.entries(this.desertAreasMapTable)) {
      const cell = Object.values(cells).find((candidateCell) => candidateCell.label === label);

      if (cell !== undefined) {
        return { locationId, cell };
      }
    }

    return null;
  }

  public static getCell(locationId: string, mapVariantKey: string): DesertAreaMapCellRaw | null {
    return this.desertAreasMapTable[locationId]?.[mapVariantKey] ?? null;
  }
}
