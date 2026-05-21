import type { DigimonSlot } from '../../models';
import type { DigimonSlotDTO } from '../../events/dto/parties/digimon-slot.dto';
import { DigimonSlotConverter } from '../../events/converters/digimon-slot.converter';
import { DigimonConverter } from '../../events/converters/digimon.converter';
import { DigimonSyncer } from './digimon.syncer';

export class DigimonSlotSyncer {
    public static sync(slots: DigimonSlot[], index: number, slotDto: DigimonSlotDTO): void {
        const previousSlot = slots[index];

        if (slotDto.digimonId === undefined || slotDto.digimonId === null || !slotDto.digimon) {
            slots[index] = {
                index: index,
                digimonId: null,
                digimon: null
            };
        } else {
            if (!previousSlot || previousSlot.digimonId !== slotDto.digimonId) {
                slots[index] = DigimonSlotConverter.convert(slotDto)!;
            } else {
                const digimonDto = slotDto.digimon;
                if (digimonDto) {
                    const previousDigimon = previousSlot.digimon;
                    if (!previousDigimon) {
                        previousSlot.digimon = DigimonConverter.convert(digimonDto);
                    } else {
                        DigimonSyncer.sync(previousDigimon, digimonDto);
                    }
                }
            }
        }
    }
}
