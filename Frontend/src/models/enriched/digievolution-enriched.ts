import type { Attributes } from '../attributes';
import type { Resistances } from '../resistances';
import type { Technique } from '../technique';

export interface EnrichedDigievolution {
    id: number;
    name: string;
    level: number;
    attributes: Attributes;
    resistances: Resistances;
    techniques: Technique[];
}
