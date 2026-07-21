import SeabedDirectionsJson from "@/database/seabed/seabed-direction.json";
import type { SeabedDirectionTable } from "./tables/seabed/seabed-direction.table";

export class SeabedDirectionsRepository {
  private static readonly seabedDirectionsTable = SeabedDirectionsJson as SeabedDirectionTable;

  public static getAll(): SeabedDirectionTable {
    return this.seabedDirectionsTable;
  }
}
