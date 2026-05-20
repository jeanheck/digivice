export interface Requisite {
    description: string;
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
    isCompleted: boolean;
    prerequisites?: Requisite[];
    locationOnMap?: string;
    locationOnMapCoordinates?: MapCoordinates;
    locations?: StepLocation[];
}

export interface Quest {
    id: string; // The GUID from backend
    title: string;
    description?: string;
    prerequisites: Requisite[];
    steps: Step[];
}

export interface Journal {
    mainQuest: Quest | null;
    sideQuests: Quest[];
}
