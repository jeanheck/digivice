import type { StoredDigievolution } from "@/models";
import type { StoredDigievolutionDTO } from "@/events/dto/parties/digimons/stored-digievolution.dto";

export class StoredDigievolutionSyncer {
    public static sync(
        previousStoredDigievolution: StoredDigievolution,
        storedDigievolutionDto: StoredDigievolutionDTO
    ): void {
        if (storedDigievolutionDto.level !== undefined) {
            previousStoredDigievolution.level = storedDigievolutionDto.level;
        }
    }
}
