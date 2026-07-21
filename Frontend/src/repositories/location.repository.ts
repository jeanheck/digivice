import LocationJson from "@/database/location/location.json";
import type { LocationTable } from "./tables/location/location.table";
import type { LocationRaw } from "./tables/raws/location/location.raw";

export class LocationRepository {
  private static readonly locationTable = LocationJson as LocationTable;

  public static getLocationById(id: string): LocationRaw {
    return this.locationTable[id] ?? {
      enemies: [],
      imageName: "",
    };
  }
}
