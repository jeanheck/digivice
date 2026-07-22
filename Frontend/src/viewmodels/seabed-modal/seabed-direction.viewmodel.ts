import type { SeabedDirectionDockViewModel } from "./seabed-route-dock.viewmodel";
import type { SeabedDirectionRouteSegmentViewModel } from "./seabed-direction-route-segment.viewmodel";

export interface SeabedDirectionViewModel {
  id: string;
  docks: SeabedDirectionDockViewModel[];
  segments: SeabedDirectionRouteSegmentViewModel[];
}
