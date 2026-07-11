import type { SeabedRouteDockViewModel } from "./seabed-route-dock.viewmodel";

export interface SeabedDirectionViewModel {
  id: string;
  docks: SeabedRouteDockViewModel[];
}
