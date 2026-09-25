import type { SeabedRouteLocationRaw } from "./seabed-route-location.raw";

export interface SeabedRouteEnemiesRaw {
  walking?: string[];
}

export interface SeabedRouteRaw {
  enemies: SeabedRouteEnemiesRaw;
  maps: Record<string, SeabedRouteLocationRaw>;
}
