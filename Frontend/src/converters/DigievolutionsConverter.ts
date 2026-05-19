import type * as DTO from '../dtos/events.dto';
import type { Digievolution } from '../models/Digimon';
import { DigievolutionRegistry } from '../logic/DigievolutionRegistry';
import { TechniqueCalculator } from '../logic/TechniqueCalculator';

export class DigievolutionsConverter {
    public static convert(digievolutions: DTO.DigievolutionSlotDTO[] | null): (Digievolution | null)[] {
        const result: (Digievolution | null)[] = Array(8).fill(null);
        
        if (!digievolutions) return result;
        
        digievolutions.forEach(slot => {
            if (slot && slot.index >= 0 && slot.index < result.length) {
                const evoId = slot.digievolutionId;
                const level = slot.digievolution?.level;
                
                if (evoId !== undefined && evoId !== null && level !== undefined && level !== null) {
                    result[slot.index] = {
                        id: evoId,
                        level,
                        name: DigievolutionRegistry.getDigievolutionNameById(evoId),
                        techniques: TechniqueCalculator.getTechniquesForDigievolution(evoId, level)
                    };
                }
            }
        });
        
        return result;
    }
}
