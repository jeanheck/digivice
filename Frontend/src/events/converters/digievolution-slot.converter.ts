import type { DigievolutionSlotDTO } from '../dto/parties/digimons/digievolution-slot.dto';
import type { DigievolutionSlot } from '../../models/Digimon';
import { DigievolutionRegistry } from '../../logic/DigievolutionRegistry';
import { TechniqueCalculator } from '../../logic/TechniqueCalculator';

export class DigievolutionSlotConverter {
    public static convert(slot: DigievolutionSlotDTO | null): DigievolutionSlot | null {
        if (!slot || slot.digievolutionId === undefined || slot.digievolutionId === null) return null;
        
        const level = slot.digievolution?.level;
        const digievolution = (level !== undefined && level !== null) ? {
            id: slot.digievolutionId,
            level,
            name: DigievolutionRegistry.getDigievolutionNameById(slot.digievolutionId),
            techniques: TechniqueCalculator.getTechniquesForDigievolution(slot.digievolutionId, level)
        } : null;

        return {
            index: slot.index,
            digievolutionId: slot.digievolutionId,
            digievolution
        };
    }
}
