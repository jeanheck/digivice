import type { DigievolutionDTO } from "@/events/dto/parties/digimons/digievolution.dto";
import type { Digievolution } from "@/models";

export class DigievolutionConverter {
    public static convert(digievolutionDto: DigievolutionDTO): Digievolution {
        return {
            level: digievolutionDto.level ?? 1,
            dvxp: digievolutionDto.dvxp ?? 0
        };
    }
}
