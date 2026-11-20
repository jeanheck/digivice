import type { MapNpcViewModel } from "./map-npc.viewmodel";

export interface AsukaServerMapViewModel {
  locationId: string;
  enemies: string[];
  boss: string[];
  fishing: string[];
  kickingTree: string[];
  npcs: MapNpcViewModel[];
}
