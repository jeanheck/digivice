import SeabedDirectionsJson from "@/database/seabed/seabed-directions.json";
import type { SeabedDirectionsTable as SeabedDirectionsTable } from "./tables/seabed/seabed-directions.table";

export class SeabedDirectionsRepository {
  private static readonly seabedDirectionsTable = SeabedDirectionsJson as SeabedDirectionsTable;

  public static getAll(): SeabedDirectionsTable {
    return this.seabedDirectionsTable;
  }
}
