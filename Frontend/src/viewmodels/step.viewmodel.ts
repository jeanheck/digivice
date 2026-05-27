import type { CoordinatesViewModel } from "./coordinates.viewmodel";
import type { RequisiteViewModel } from "./requisite.viewmodel";
import type { ZoomedLocationViewModel } from "./zoomed-location.viewmodel";

export interface StepViewModel
{
    number: string;
    requisites: RequisiteViewModel[];
    isDone: boolean;
    location: string;
    coordinates: CoordinatesViewModel;
    zoomedLocations: ZoomedLocationViewModel[];
}