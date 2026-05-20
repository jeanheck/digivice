import type { Vitals } from './vitals';
import type { Attributes } from './attributes';
import type { Resistances } from './resistances';
import type { Equipments } from './equipments';
import type { DigievolutionSlot } from './digievolution-slot';

export interface Digimon {
    level: number;
    experience: number;
    vitals: Vitals;
    attributes: Attributes;
    resistances: Resistances;
    equipments: Equipments;
    digievolutions: DigievolutionSlot[];
    activeDigievolutionId: number | null;
}
