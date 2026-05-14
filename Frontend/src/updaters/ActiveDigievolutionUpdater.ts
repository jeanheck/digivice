import type { Digimon } from '../models/Digimon';

export class ActiveDigievolutionUpdater {
    public static update(digimon: Digimon, newActiveDigievolutionId: number | null): void {
        digimon.activeDigievolutionId = newActiveDigievolutionId;
    }
}
