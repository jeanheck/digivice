import type { Party } from '../models/Player';
import type { Digimon } from '../models/Digimon';

export class PartyUpdater {
    public static update(party: Party, slots: (Digimon | null)[]) {
        party.slots = slots;
    }
}
