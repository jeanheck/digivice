import type { Digimon, Resistances } from '../models/Digimon';

export class ResistancesUpdater {
    /**
     * Pure state mutation. Replaces the Resistances object of a specific Digimon.
     */
    public static update(digimon: Digimon, newResistances: Resistances): void {
        digimon.resistances = newResistances;
    }
}
