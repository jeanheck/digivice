import type { SeabedDirection } from "@/shared/types/seabed-direction.type";

export interface SeabedRouteEmergeRaw {
  on: SeabedDirection;
  location: string;
}
