import type * as DTO from '../dtos/events.dto';
import type { Party } from '../models/Player';
import { DigimonConverter } from './DigimonConverter';
import { PartyCalculator } from '../logic/PartyCalculator';

export class PartyConverter {
    public static convert(party: DTO.PartyDTO | null): Party | null {
        if (!party) return null;
        
        const slots = party.slots.map(slot => DigimonConverter.convert(slot));
        
        return {
            slots,
            groupCharisma: PartyCalculator.calculateGroupCharisma(slots)
        };
    }
}
