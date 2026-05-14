import type { Resistances } from '../models/Digimon';

export class ResistancesConverter {
    /**
     * Creates a new Resistances object by merging the current state with the incoming overrides.
     */
    public static convert(currentResistances: Resistances, overrides: Partial<Resistances>): Resistances {
        return { ...currentResistances, ...overrides };
    }
}
