import type { SeabedRouteDockViewModel } from "./seabed-route-dock.viewmodel";

export interface SeabedRouteViewModel {
  id: string;
  docks: SeabedRouteDockViewModel[];
}
