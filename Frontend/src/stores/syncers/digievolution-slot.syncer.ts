import type { DigievolutionSlot } from '../../models';
import type { DigievolutionSlotDTO } from '../../events/dto/parties/digimons/digievolution-slot.dto';
import { DigievolutionSyncer } from './digievolution.syncer';

export class DigievolutionSlotSyncer {
    public static sync(slots: DigievolutionSlot[], index: number, slotDto: DigievolutionSlotDTO): void {
        const previousSlot = slots[index];
        if (!previousSlot) {
            return;
        }

        if (slotDto.digievolutionId !== undefined) {
            previousSlot.digievolutionId = slotDto.digievolutionId;
        }

        if (slotDto.digievolution === undefined || slotDto.digievolution === null) {
            previousSlot.digievolution = null;
        } else {
            const digievolutionDto = slotDto.digievolution;
            const previousDigievolution = previousSlot.digievolution;

            if (!previousDigievolution) {
                previousSlot.digievolution = {
                    level: digievolutionDto.level ?? 0
                };
            } else {
                DigievolutionSyncer.sync(previousDigievolution, digievolutionDto);
            }
        }
    }
}
