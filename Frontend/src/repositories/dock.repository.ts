import DocksJson from "@/database/seabed/seabed-docks.json";
import type { DockTable } from "./tables/dock/dock.table";
import type { DockRaw } from "./tables/raws/dock/dock.raw";

export class DockRepository {
  private static readonly dockTable = DocksJson as DockTable;

  public static getDockByLocationId(locationId: string): DockRaw | null {
    return this.dockTable[locationId] ?? null;
  }
}
