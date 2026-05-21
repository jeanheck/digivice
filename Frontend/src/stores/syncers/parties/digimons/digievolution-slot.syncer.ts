import type { DigievolutionSlot } from '@/models';
import type { DigievolutionSlotDTO } from '@/events/dto/parties/digimons/digievolution-slot.dto';
import { DigievolutionSyncer } from './digievolution.syncer';
import { DigievolutionConverter } from '@/events/converters/parties/digimons/digievolution.converter';

export class DigievolutionSlotSyncer {
    public static sync(previousDigievolutionSlot: DigievolutionSlot, newDigievolutionSlotDto: DigievolutionSlotDTO): void {
        const newId = newDigievolutionSlotDto.digievolutionId;
        const newDigievolution = newDigievolutionSlotDto.digievolution;

        if (newId === null || newDigievolution === null) {
            previousDigievolutionSlot.digievolutionId = null;
            previousDigievolutionSlot.digievolution = null;
            return;
        }

        if (newId !== undefined && newDigievolution !== undefined) {
            previousDigievolutionSlot.digievolutionId = newId;

            const previousDigievolution = previousDigievolutionSlot.digievolution;
            if (!previousDigievolution) {
                previousDigievolutionSlot.digievolution = DigievolutionConverter.convert(newDigievolution);
                return;
            }

            DigievolutionSyncer.sync(previousDigievolution, newDigievolution);
        }
    }
}
