import type { DigievolutionSlotDTO } from '../dto/parties/digimons/digievolution-slot.dto';
import type { Digievolution } from '../../models/Digimon';
import { DigievolutionSlotConverter } from './digievolution-slot.converter';

export class DigievolutionsConverter {
    public static convert(digievolutions: DigievolutionSlotDTO[] | null): (Digievolution | null)[] {
        const result: (Digievolution | null)[] = Array(8).fill(null);

        if (!digievolutions) return result;

        digievolutions.forEach(slot => {
            if (slot && slot.index >= 0 && slot.index < result.length) {
                result[slot.index] = DigievolutionSlotConverter.convert(slot);
            }
        });

        return result;
    }
}
