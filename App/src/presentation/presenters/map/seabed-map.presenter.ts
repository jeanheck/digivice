import { MapService } from "@/presentation/services/map.service";

export class SeabedMapPresenter {
  public static getEnemyIds(seabedRoute: number | null): string[] {
    return MapService.getSeabedEnemies(seabedRoute);
  }
}
