import type { Digimon, Resistances } from '../models';

export class ResistancesUpdater {
    public static update(digimon: Digimon, newResistances: Resistances): void {
        digimon.resistances = newResistances;
    }
}
