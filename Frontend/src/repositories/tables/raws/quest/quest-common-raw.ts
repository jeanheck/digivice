export interface CoordinatesRaw {
    X: number;
    Y: number;
}

export interface ZoomedLocationRaw {
    LocationImage: string;
    Coordinates: CoordinatesRaw;
}

export interface RequisiteRaw {
    Id: string;
}

export interface StepRaw {
    Location: string;
    Coordinates: CoordinatesRaw;
    ZoomedLocations: ZoomedLocationRaw[];
    Requisites?: RequisiteRaw[];
}

export type StepsRaw = Record<string, StepRaw>;
