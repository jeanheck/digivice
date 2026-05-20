import type { DigievolutionSlotDTO } from '../dto/parties/digimons/digievolution-slot.dto';
import type { DigievolutionSlot } from '../../models';

export class DigievolutionSlotConverter {
    public static convert(slot: DigievolutionSlotDTO | null): DigievolutionSlot | null {
        if (!slot || slot.digievolutionId === undefined || slot.digievolutionId === null) return null;
        
        const level = slot.digievolution?.level;
        const digievolution = (level !== undefined && level !== null) ? {
            level
        } : null;

        return {
            index: slot.index,
            digievolutionId: slot.digievolutionId,
            digievolution
        };
    }
}
