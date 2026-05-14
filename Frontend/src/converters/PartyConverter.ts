import type * as DTO from '../dtos/events.dto';
import type { Party } from '../models/Player';
import { DigimonConverter } from './DigimonConverter';

export class PartyConverter {
    public static convert(party: DTO.PartyDTO | null): Party | null {
        if (!party) return null;
        return {
            slots: party.slots.map(slot => DigimonConverter.convert(slot))
        };
    }
}
