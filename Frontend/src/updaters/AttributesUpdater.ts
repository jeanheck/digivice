import type { Digimon, Attributes } from '../models/Digimon';

export class AttributesUpdater {
    public static update(digimon: Digimon, newAttributes: Attributes): void {
        digimon.attributes = newAttributes;
    }
}
