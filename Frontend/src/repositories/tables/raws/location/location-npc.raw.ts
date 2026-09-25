import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import type { LocationMapLabelPlacementRaw } from "@/repositories/tables/raws/location/location-map-label-placement.raw";

export interface LocationNpcRaw {
  id: string;
  coordinates?: CoordinatesRaw;
  labelPlacement?: LocationMapLabelPlacementRaw;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
}
