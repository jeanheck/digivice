import type { Digimon, Digievolution } from '../models/Digimon';

export class EquippedDigievolutionsUpdater {
    public static update(digimon: Digimon, newDigievolutions: (Digievolution | null)[]) {
        digimon.equippedDigievolutions = newDigievolutions;
    }
}
