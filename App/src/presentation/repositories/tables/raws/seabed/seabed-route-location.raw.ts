import type { SeabedRouteEmergeRaw } from "./seabed-route-emerge.raw";

export interface SeabedRouteLocationRaw {
  emerge: SeabedRouteEmergeRaw[];
  left: string[];
  right: string[];
}
