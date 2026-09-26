import type { DigievolutionSlot } from "@/models";
import type { DeepRequired } from "@/events/dto/deep-required";
import type { DigievolutionDTO } from "@/events/dto/parties/digimons/digievolution.dto";
import type { DigievolutionSlotDTO } from "@/events/dto/parties/digimons/digievolution-slot.dto";
import { DigievolutionSyncer } from "./digievolution.syncer";
import { DigievolutionConverter } from "@/events/converters/parties/digimons/digievolution.converter";
import { storeLogger } from "@/events/logger";

export class DigievolutionSlotSyncer {
  public static sync(
    previousDigievolutionSlot: DigievolutionSlot,
    newDigievolutionSlotDto: DigievolutionSlotDTO,
  ): void {
    const newId = newDigievolutionSlotDto.digievolutionId;
    const newDigievolution = newDigievolutionSlotDto.digievolution;

    if (newId === null || newDigievolution === null) {
      storeLogger.warn(
        `Digievolution slot ${newDigievolutionSlotDto.index}: filled → empty transition ignored.`,
      );
      return;
    }

    if (newId !== undefined && newDigievolution !== undefined) {
      previousDigievolutionSlot.digievolutionId = newId;
      // Backend sends the full digievolution when the slot id changes.
      previousDigievolutionSlot.digievolution = DigievolutionConverter.convert(
        newDigievolution as DeepRequired<DigievolutionDTO>,
      );
      return;
    }

    if (newDigievolution !== undefined) {
      const previousDigievolution = previousDigievolutionSlot.digievolution;
      if (previousDigievolution) {
        DigievolutionSyncer.sync(previousDigievolution, newDigievolution);
        return;
      }
    }
  }
}
