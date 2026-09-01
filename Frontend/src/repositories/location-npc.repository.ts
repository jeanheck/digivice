import { LocationRepository } from "@/repositories/location.repository";
import type { LocationNpcRaw } from "@/repositories/tables/raws/location/location-npc.raw";
import { NpcService } from "@/services/npc.service";

export class LocationNpcRepository {
  public static getByLocationId(locationId: string): LocationNpcRaw[] {
    return LocationRepository.getLocationById(locationId).npcs ?? [];
  }

  public static getIdsByLocationId(
    locationId: string,
    lastCompletedMainQuestStep: number,
  ): string[] {
    return this.getByLocationId(locationId).flatMap((locationNpcRaw) => {
      if (
        !NpcService.isVisibleOnMapByMainQuestStep(
          lastCompletedMainQuestStep,
          locationNpcRaw.mainQuestStepDone,
        )
      ) {
        return [];
      }

      return [locationNpcRaw.id];
    });
  }
}
