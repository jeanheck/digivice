import type { Party } from '../../models';
import type { PartyDTO } from '../../events/dto/party.dto';
import { DigimonSlotSyncer } from './digimon-slot.syncer';

export class PartySyncer {
    public static sync(previousParty: Party, partyDto: PartyDTO): void {
        if (partyDto.slots && partyDto.slots.length > 0) {
            partyDto.slots.forEach((slotDto) => {
                if (slotDto) {
                    const index = slotDto.index;
                    if (index >= 0 && index < previousParty.slots.length) {
                        DigimonSlotSyncer.sync(previousParty.slots, index, slotDto);
                    }
                }
            });
        }
    }
}
