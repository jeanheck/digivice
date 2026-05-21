import type { DigievolutionSlotDTO } from "@/events/dto/parties/digimons/digievolution-slot.dto";
import type { DigievolutionSlot } from "@/models";
import { DigievolutionConverter } from "./digievolution.converter";

export class DigievolutionSlotConverter {
    public static convert(newDigievolutionSlotDto: DigievolutionSlotDTO): DigievolutionSlot {
        return {
            index: newDigievolutionSlotDto.index,
            digievolutionId: newDigievolutionSlotDto.digievolutionId ?? null,
            digievolution: newDigievolutionSlotDto.digievolution ? DigievolutionConverter.convert(newDigievolutionSlotDto.digievolution) : null
        };
    }
}
