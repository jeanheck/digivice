import type { DigievolutionDTO } from '../dto/parties/digimons/digievolution.dto';
import type { Digievolution } from '../../models';

export class DigievolutionConverter {
    public static convert(digievolutionDto: DigievolutionDTO): Digievolution {
        return {
            level: digievolutionDto.level
        };
    }
}
