import type { DigimonSlot } from '../../../models';
import type { DigimonSlotDTO } from '../../../events/dto/parties/digimon-slot.dto';
import { DigimonConverter } from '../../../events/converters/parties/digimon.converter';
import { DigimonSyncer } from './digimon.syncer';

export class DigimonSlotSyncer {
    public static sync(previousDigimonSlot: DigimonSlot, newDigimonSlotDto: DigimonSlotDTO): void {
        const newId = newDigimonSlotDto.digimonId;
        const newDigimon = newDigimonSlotDto.digimon;

        if (newId === null || newDigimon === null) {
            previousDigimonSlot.digimonId = null;
            previousDigimonSlot.digimon = null;
            return;
        }

        if (newId !== undefined && newDigimon !== undefined) {
            if (!previousDigimonSlot.digimon) {
                previousDigimonSlot.digimonId = newId;
                previousDigimonSlot.digimon = DigimonConverter.convert(newDigimon);
                return;
            }

            previousDigimonSlot.digimonId = newId;
            DigimonSyncer.sync(previousDigimonSlot.digimon, newDigimon);
        }
    }
}
