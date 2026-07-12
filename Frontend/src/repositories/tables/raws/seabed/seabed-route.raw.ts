import type { SeabedRouteLocationRaw } from "./seabed-route-location.raw";

export interface SeabedRouteRaw {
  enemies: string[];
  maps: Record<string, SeabedRouteLocationRaw>;
}
