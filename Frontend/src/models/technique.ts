import type { TechniqueType } from './technique-type';

export interface Technique {
    type: TechniqueType;
    element: string;
    elementStrength: number;
    mp: number;
    power: number;
}
