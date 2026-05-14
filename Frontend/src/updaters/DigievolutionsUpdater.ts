import type { Digimon, Digievolution } from '../models/Digimon';

export class DigievolutionsUpdater {
    public static update(digimon: Digimon, newDigievolutions: (Digievolution | null)[]) {
        digimon.digievolutions = newDigievolutions;
    }
}
