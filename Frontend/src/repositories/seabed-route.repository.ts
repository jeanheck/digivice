import SeabedRoutesJson from "@/database/docks/seabed-routes.json";
import type { SeabedRoutesTable } from "./tables/dock/seabed-routes.table";

export class SeabedRouteRepository {
  private static readonly seabedRoutesTable = SeabedRoutesJson as SeabedRoutesTable;

  public static getAll(): SeabedRoutesTable {
    return this.seabedRoutesTable;
  }
}
