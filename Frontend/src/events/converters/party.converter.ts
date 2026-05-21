import type { PartyDTO } from '../dto/party.dto';
import type { Party, DigimonSlot } from '../../models';
import { DigimonSlotConverter } from './parties/digimon-slot.converter';

export class PartyConverter {
    public static convert(partyDto: Required<PartyDTO>): Party {
        return {
            slots: partyDto.slots.map(slot => DigimonSlotConverter.convert(slot))
        };
    }
}
