import type { DigimonSlotDTO } from '../dto/parties/digimon-slot.dto';
import type { DigimonSlot } from '../../models';
import { DigimonConverter } from './digimon.converter';

export class DigimonSlotConverter {
    public static convert(slotDto: DigimonSlotDTO | null): DigimonSlot | null {
        if (!slotDto) return null;
        return {
            index: slotDto.index,
            digimonId: slotDto.digimonId ?? null,
            digimon: slotDto.digimon ? DigimonConverter.convert(slotDto.digimon) : null
        };
    }
}
