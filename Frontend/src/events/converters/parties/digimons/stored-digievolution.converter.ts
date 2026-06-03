import type { StoredDigievolutionDTO } from "@/events/dto/parties/digimons/stored-digievolution.dto";
import type { StoredDigievolution } from "@/models";

export class StoredDigievolutionConverter {
    public static convert(storedDigievolutionDto: StoredDigievolutionDTO): StoredDigievolution {
        return {
            digievolutionId: storedDigievolutionDto.digievolutionId ?? 0,
            level: storedDigievolutionDto.level ?? 1
        };
    }
}
