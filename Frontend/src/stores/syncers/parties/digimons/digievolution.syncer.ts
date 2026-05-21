import type { Digievolution } from '../../models';
import type { DigievolutionDTO } from '../../events/dto/parties/digimons/digievolution.dto';

export class DigievolutionSyncer {
    public static sync(previousDigievolution: Digievolution, digievolutionDto: DigievolutionDTO): void {
        if (digievolutionDto.level !== undefined) {
            previousDigievolution.level = digievolutionDto.level;
        }
    }
}
