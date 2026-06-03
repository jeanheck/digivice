import type { Vitals } from './vitals';
import type { Attributes } from './attributes';
import type { Resistances } from './resistances';
import type { Equipments } from './equipments';
import type { DigievolutionSlot } from './digievolution-slot';
import type { StoredDigievolution } from './stored-digievolution';

export interface Digimon {
    level: number;
    experience: number;
    vitals: Vitals;
    attributes: Attributes;
    resistances: Resistances;
    equipments: Equipments;
    digievolutions: DigievolutionSlot[];
    storedDigievolutions: StoredDigievolution[];
    activeDigievolutionId: number | null;
}
