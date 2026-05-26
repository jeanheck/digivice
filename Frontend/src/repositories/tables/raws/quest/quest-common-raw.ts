export interface CoordinatesRaw {
    x: number;
    y: number;
}

export interface ZoomedLocationRaw {
    locationImage: string;
    coordinates: CoordinatesRaw;
}

export interface RequisiteRaw {
    id: string;
}

export interface StepRaw {
    requisites: RequisiteRaw[];
    location: string;
    coordinates: CoordinatesRaw;
    zoomedLocations: ZoomedLocationRaw[];
}

export type StepsRaw = Record<string, StepRaw>;
