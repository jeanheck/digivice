import type { DigimonSlot } from '../../models';
import type { DigimonSlotDTO } from '../../events/dto/parties/digimon-slot.dto';
import { DigimonConverter } from '../../events/converters/digimon.converter';
import { DigimonSyncer } from './digimon.syncer';

export class DigimonSlotSyncer {
    public static sync(previousDigimonSlot: DigimonSlot, newDigimonSlotDto: DigimonSlotDTO): void {
        if (!newDigimonSlotDto.digimonId || !newDigimonSlotDto.digimon) {
            previousDigimonSlot.digimonId = null;
            previousDigimonSlot.digimon = null;
            return;
        }

        if (!previousDigimonSlot.digimon) {
            previousDigimonSlot.digimonId = newDigimonSlotDto.digimonId;
            previousDigimonSlot.digimon = DigimonConverter.convert(newDigimonSlotDto.digimon);
            return;
        }

        previousDigimonSlot.digimonId = newDigimonSlotDto.digimonId;
        DigimonSyncer.sync(previousDigimonSlot.digimon, newDigimonSlotDto.digimon);
    }
}
