import type { Attributes } from '@/models/attributes';
import type { Resistances } from '@/models/resistances';
import type { Technique } from '@/models/technique';

export interface EnrichedDigievolution {
    id: number;
    name: string;
    level: number;
    attributes: Attributes;
    resistances: Resistances;
    techniques: Technique[];
}
