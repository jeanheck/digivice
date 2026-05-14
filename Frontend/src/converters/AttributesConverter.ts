import type { Attributes } from '../models/Digimon';

export class AttributesConverter {
    /**
     * Creates a new Attributes object by merging the current state with the incoming overrides.
     */
    public static convert(currentAttributes: Attributes, overrides: Partial<Attributes>): Attributes {
        return { ...currentAttributes, ...overrides };
    }
}
