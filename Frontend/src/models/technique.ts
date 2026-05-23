import type { TechniqueType } from './technique-type';

export interface Technique {
    id: string;
    type: TechniqueType;
    element: string;
    elementStrength: number;
    mp: number;
    power: number;
}
