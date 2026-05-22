export type TechniqueTypeId = 'Physical' | 'Magical' | 'Heal' | 'Support';

export interface TechniqueType {
    id: TechniqueTypeId;
    description: Record<string, string>;
}
