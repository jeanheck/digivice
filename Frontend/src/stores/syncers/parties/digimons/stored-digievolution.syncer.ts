import type { StoredDigievolution } from "@/models";
import type { StoredDigievolutionDTO } from "@/events/dto/parties/digimons/stored-digievolution.dto";

export class StoredDigievolutionSyncer {
  public static sync(
    previousStoredDigievolution: StoredDigievolution,
    newStoredDigievolutionDto: StoredDigievolutionDTO,
  ): void {
    if (newStoredDigievolutionDto.level !== undefined) {
      previousStoredDigievolution.level = newStoredDigievolutionDto.level;
    }
  }
}
