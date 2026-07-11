import type { SeabedDirectionDockViewModel } from "./seabed-route-dock.viewmodel";

export interface SeabedDirectionViewModel {
  id: string;
  docks: SeabedDirectionDockViewModel[];
}
