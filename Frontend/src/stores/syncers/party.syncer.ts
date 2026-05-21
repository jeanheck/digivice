import type { Party } from '../../models';
import type { PartyDTO } from '../../events/dto/party.dto';
import { DigimonSlotSyncer } from './digimon-slot.syncer';

export class PartySyncer {
    public static sync(previousParty: Party, newPartyDto: PartyDTO): void {
        if (!newPartyDto.slots || newPartyDto.slots.length === 0) {
            return;
        }

        newPartyDto.slots.forEach((newSlotDto) => {
            if (newSlotDto) {
                const previousSlot = previousParty.slots.find((s) => s.index === newSlotDto.index);
                if (previousSlot) {
                    DigimonSlotSyncer.sync(previousSlot, newSlotDto);
                }
            }
        });
    }
}
