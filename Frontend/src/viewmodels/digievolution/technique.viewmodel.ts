export interface TechniqueViewModel {
    id: string;
    learnLevel: number;
    loadedLevel: number | null;
    type: string;
    element: string;
    elementStrength: number;
    mp: number;
    power: number;
    isUnlocked: boolean;
    isSignature: boolean;
}