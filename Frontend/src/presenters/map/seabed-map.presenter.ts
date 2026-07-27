import { LocationService } from "@/services/location.service";

export class SeabedMapPresenter {
  public static getEnemyIds(seabedRoute: number): string[] {
    return LocationService.getSeabedEnemies(seabedRoute);
  }
}
