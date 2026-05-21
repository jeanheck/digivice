import type { DigimonSlot, Digimon } from '../../models';

export class PartyHelper {
    public static getDigimons(slots: DigimonSlot[]): Digimon[] {
        const digimons: Digimon[] = [];
        for (const slot of slots) {
            if (slot && slot.digimon) {
                digimons.push(slot.digimon);
            }
        }
        return digimons;
    }
}
