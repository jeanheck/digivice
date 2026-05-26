import type { CoordinatesViewModel } from "./coordinates-view-model";

export interface ZoomedLocationViewModel
{
    location: string,
    coordinates: CoordinatesViewModel;
}