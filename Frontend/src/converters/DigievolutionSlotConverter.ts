import type { DigievolutionSlotDTO } from '../events/dto/Parties/Digimons/DigievolutionSlotDTO';
import type { Digievolution } from '../models/Digimon';
import { DigievolutionRegistry } from '../logic/DigievolutionRegistry';
import { TechniqueCalculator } from '../logic/TechniqueCalculator';

export class DigievolutionSlotConverter {
    public static convert(slot: DigievolutionSlotDTO | null): Digievolution | null {
        if (!slot || slot.digievolutionId === undefined || slot.digievolutionId === null) return null;
        const level = slot.digievolution?.level;
        if (level === undefined || level === null) return null;
        
        return {
            id: slot.digievolutionId,
            level,
            name: DigievolutionRegistry.getDigievolutionNameById(slot.digievolutionId),
            techniques: TechniqueCalculator.getTechniquesForDigievolution(slot.digievolutionId, level)
        };
    }
}
