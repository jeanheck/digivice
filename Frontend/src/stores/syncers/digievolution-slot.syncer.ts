import type { DigievolutionSlot } from '../../models';
import type { DigievolutionSlotDTO } from '../../events/dto/parties/digimons/digievolution-slot.dto';
import { DigievolutionConverter } from '../../events/converters/digievolution.converter';
import { DigievolutionSyncer } from './digievolution.syncer';

export class DigievolutionSlotSyncer {
    public static sync(previousDigievolutionSlot: DigievolutionSlot, newDigievolutionSlotDto: DigievolutionSlotDTO): void {
        if (!newDigievolutionSlotDto.digievolutionId || !newDigievolutionSlotDto.digievolution) {
            return;
        }

        previousDigievolutionSlot.digievolutionId = newDigievolutionSlotDto.digievolutionId;

        const previousDigievolution = previousDigievolutionSlot.digievolution;
        if (!previousDigievolution) {
            previousDigievolutionSlot.digievolution = DigievolutionConverter.convert(newDigievolutionSlotDto.digievolution);
            return;
        }

        DigievolutionSyncer.sync(previousDigievolution, newDigievolutionSlotDto.digievolution);
    }
}
