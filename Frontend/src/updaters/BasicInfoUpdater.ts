import type { Digimon, BasicInfo } from '../models/Digimon';

export class BasicInfoUpdater {
    /**
     * Pure state mutation. Replaces the BasicInfo object of a specific Digimon.
     */
    public static update(digimon: Digimon, newBasicInfo: BasicInfo) {
        digimon.basicInfo = newBasicInfo;
    }
}
