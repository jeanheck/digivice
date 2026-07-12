import SeabedRoutesJson from "@/database/seabed/seabed-routes.json";
import type { SeabedRouteLocationRaw } from "./tables/raws/seabed/seabed-route-location.raw";
import type { SeabedRoutesTable } from "./tables/seabed/seabed-routes.table";

export class SeabedRoutesRepository {
  private static readonly seabedRoutesTable = SeabedRoutesJson as SeabedRoutesTable;

  public static getAll(): SeabedRoutesTable {
    return this.seabedRoutesTable;
  }

  public static getByRouteAndLocation(routeId: string, locationKey: string): SeabedRouteLocationRaw | null {
    return this.seabedRoutesTable[routeId]?.[locationKey] ?? null;
  }
}
