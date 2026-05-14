import type { Party } from '../models/Player';
import type { Digimon } from '../models/Digimon';

export class PartySlotsUpdater {
    public static update(party: Party, newSlots: (Digimon | null)[]) {
        party.slots = newSlots;
    }
}
