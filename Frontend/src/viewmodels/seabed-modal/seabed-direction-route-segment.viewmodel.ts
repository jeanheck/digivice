import type { SeabedDirectionDockViewModel } from "./seabed-route-dock.viewmodel";

export interface SeabedDirectionRouteSegmentViewModel {
  from: SeabedDirectionDockViewModel;
  to: SeabedDirectionDockViewModel;
}
