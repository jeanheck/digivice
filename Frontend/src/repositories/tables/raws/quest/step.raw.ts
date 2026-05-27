import type { CoordinatesRaw } from "./coordinates.raw";
import type { RequisiteRaw } from "./requisite.raw";
import type { ZoomedLocationRaw } from "./zoomed-location.raw";

export interface StepRaw {
    requisites: RequisiteRaw[];
    location: string;
    coordinates: CoordinatesRaw;
    zoomedLocations: ZoomedLocationRaw[];
}