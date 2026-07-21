import DesertAreasMapJson from "@/database/mobius-desert/mobius-desert-areas-map.json";
import type { MobiusDesertAreaMapCellRaw } from "./tables/raws/mobius-desert/mobius-desert-area-map-cell.raw";
import type { MobiusDesertAreasMapTable } from "./tables/desert/mobius-desert-areas-map.table";

export class MobiusDesertAreasMapRepository {
  private static readonly desertAreasMapTable = DesertAreasMapJson as MobiusDesertAreasMapTable;

  public static findByLabel(label: string): { locationId: string; cell: MobiusDesertAreaMapCellRaw } | null {
    for (const [locationId, cells] of Object.entries(this.desertAreasMapTable)) {
      const cell = Object.values(cells).find((candidateCell) => candidateCell.label === label);

      if (cell !== undefined) {
        return { locationId, cell };
      }
    }

    return null;
  }

  public static getCell(locationId: string, mapVariantKey: string): MobiusDesertAreaMapCellRaw | null {
    return this.desertAreasMapTable[locationId]?.[mapVariantKey] ?? null;
  }
}
