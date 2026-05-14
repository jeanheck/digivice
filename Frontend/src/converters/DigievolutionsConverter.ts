import type * as DTO from '../dtos/events.dto';
import type { Digievolution } from '../models/Digimon';
import { DigievolutionRegistry } from '../logic/DigievolutionRegistry';

export class DigievolutionsConverter {
    public static convert(digievolutions: (DTO.DigievolutionDTO | null)[]): (Digievolution | null)[] {
        return digievolutions.map(digievolution => {
            if (!digievolution) return null;
            return {
                id: digievolution.id,
                level: digievolution.level,
                name: DigievolutionRegistry.getDigievolutionNameById(digievolution.id)
            };
        });
    }
}
