import DocksJson from "@/database/seabed/seabed-dock.json";
import type { DockTable } from "./tables/seabed/dock.table";
import type { DockRaw } from "./tables/raws/seabed/dock.raw";

export class SeabedDockRepository {
  private static readonly dockTable = DocksJson as DockTable;

  public static getDockByLocationId(locationId: string): DockRaw | null {
    return this.dockTable[locationId] ?? null;
  }
}
