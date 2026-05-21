import type { DigimonSlot, Digimon } from '../../models';

export class PartyHelper {
    public static getDigimons(slots: DigimonSlot[]): Digimon[] {
        return slots
            .map(slot => slot.digimon)
            .filter(digimon => digimon !== null);
    }
}
