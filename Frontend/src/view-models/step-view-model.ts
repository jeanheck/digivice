import type { CoordinatesViewModel } from "./coordinates-view-model";
import type { RequisiteViewModel } from "./requisite-view-model";
import type { ZoomedLocationViewModel } from "./zoomed-location-view-model";

export interface StepViewModel
{
    number: string;
    requisites: RequisiteViewModel[];
    isDone: boolean;
    location: string;
    coordinates: CoordinatesViewModel;
    zoomedLocations: ZoomedLocationViewModel[];
}