import type { CoordinatesViewModel } from "./coordinates.viewmodel";
import type { InnerLocationViewModel } from "./inner-location.viewmodel";
import type { RequisiteViewModel } from "./requisite.viewmodel";

export interface StepViewModel {
  number: string;
  requisites: RequisiteViewModel[];
  isDone: boolean;
  location: string;
  coordinates: CoordinatesViewModel;
  innerLocation: InnerLocationViewModel[];
}
