import MapJson from "@/database/map.json";
import type { MapTable } from "./tables/map/map.table";
import type { MapRaw } from "./tables/raws/map/map.raw";

export class MapRepository {
  private static readonly mapTable = MapJson as MapTable;

  public static getMapById(id: string): MapRaw {
    return (
      this.mapTable[id] ?? {
        imageName: "",
      }
    );
  }

  public static getMapIdsWithWorldLocation(): string[] {
    return Object.entries(this.mapTable)
      .filter(([, mapRaw]) => {
        return mapRaw.worldLocation !== undefined;
      })
      .map(([mapId]) => {
        return mapId;
      });
  }
}
