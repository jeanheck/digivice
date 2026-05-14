import type { Digimon, Resistances } from '../models/Digimon';

export class ResistancesUpdater {
    public static update(digimon: Digimon, newResistances: Resistances): void {
        digimon.resistances = newResistances;
    }
}
