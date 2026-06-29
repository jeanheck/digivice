import type { CoordinatesViewModel } from "./coordinates.viewmodel";

export interface ZoomedLocationViewModel
{
    location: string,
    coordinates: CoordinatesViewModel;
}