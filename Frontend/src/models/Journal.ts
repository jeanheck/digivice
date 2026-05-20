export interface Requisite {
    id: string;
    isDone: boolean;
}

export interface MapCoordinates {
    x: number;
    y: number;
}

export interface StepLocation {
    locationImage?: string;
    target?: string;
    locationImageCoordinates?: MapCoordinates;
}

export interface Step {
    number: number;
    description?: string; // Enriched via local table
    isDone: boolean;
    requisites?: Requisite[];
    locationOnMap?: string;
    locationOnMapCoordinates?: MapCoordinates;
    locations?: StepLocation[];
}

export interface Quest {
    id: string; // The GUID from backend
    requisites: Requisite[];
    steps: Step[];
}

export interface Journal {
    mainQuest: Quest | null;
    sideQuests: Quest[];
}
