import type { DeepRequired } from "@/events/dto/deep-required";
import type { DigievolutionSlotDTO } from "@/events/dto/parties/digimons/digievolution-slot.dto";
import type { DigievolutionSlot } from "@/models";
import { DigievolutionConverter } from "./digievolution.converter";

export class DigievolutionSlotConverter {
  public static convert(digievolutionSlotDto: DeepRequired<DigievolutionSlotDTO>): DigievolutionSlot {
    return {
      index: digievolutionSlotDto.index,
      digievolutionId: digievolutionSlotDto.digievolutionId,
      digievolution: digievolutionSlotDto.digievolution
        ? DigievolutionConverter.convert(digievolutionSlotDto.digievolution)
        : null,
    };
  }
}
