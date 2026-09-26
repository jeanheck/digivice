import { LocationService } from "@/services/location.service";

export class SeabedMapPresenter {
  public static getEnemyIds(seabedRoute: number | null): string[] {
    return LocationService.getSeabedEnemies(seabedRoute);
  }
}
