import type { Digimon, Attributes } from '../models/Digimon';

export class AttributesUpdater {
    /**
     * Pure state mutation. Replaces the Attributes object of a specific Digimon.
     */
    public static update(digimon: Digimon, newAttributes: Attributes): void {
        digimon.attributes = newAttributes;
    }
}
