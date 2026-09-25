import type { Digievolution } from "@/models";
import type { DigievolutionDTO } from "@/events/dto/parties/digimons/digievolution.dto";

export class DigievolutionSyncer {
  public static sync(
    previousDigievolution: Digievolution,
    newDigievolutionDto: DigievolutionDTO,
  ): void {
    if (newDigievolutionDto.level !== undefined) {
      previousDigievolution.level = newDigievolutionDto.level;
    }
    if (newDigievolutionDto.dvexp !== undefined) {
      previousDigievolution.dvexp = newDigievolutionDto.dvexp;
    }
  }
}
