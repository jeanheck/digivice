import SeabedRoutesJson from "@/database/seabed/seabed-route.json";
import type { SeabedRouteLocationRaw } from "./tables/raws/seabed/seabed-route-location.raw";
import type { SeabedRouteTable } from "./tables/seabed/seabed-route.table";

export class SeabedRoutesRepository {
  private static readonly seabedRoutesTable = SeabedRoutesJson as SeabedRouteTable;

  public static getAll(): SeabedRouteTable {
    return this.seabedRoutesTable;
  }

  public static getByRouteAndLocation(routeId: string, locationKey: string): SeabedRouteLocationRaw | null {
    return this.seabedRoutesTable[routeId]?.maps[locationKey] ?? null;
  }

  public static getEnemiesByRoute(routeId: string): string[] {
    return this.seabedRoutesTable[routeId]?.enemies ?? [];
  }
}
