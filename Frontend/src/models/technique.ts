import type { TechniqueType } from './technique-type';

export interface Technique {
    id: string;
    name: Record<string, string>;
    type: TechniqueType;
    element: string;
    elementStrength: number;
    mp: number;
    power: number;
    description: Record<string, string>;
    learnLevel: number;
    isSignature: boolean;
    isUnlocked: boolean;
}
