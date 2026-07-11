export type SeabedRouteEmergeSideViewModel = "left" | "right";

export interface SeabedRouteEmergeViewModel {
  on: SeabedRouteEmergeSideViewModel;
  location: string;
}
