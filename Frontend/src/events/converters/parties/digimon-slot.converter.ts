import type { DigimonSlotDTO } from '../../dto/parties/digimon-slot.dto';
import type { DigimonSlot } from '../../../models';
import { DigimonConverter } from './digimon.converter';

export class DigimonSlotConverter {
    public static convert(digimonSlotDto: DigimonSlotDTO): DigimonSlot {
        return {
            index: digimonSlotDto.index,
            digimonId: digimonSlotDto.digimonId ?? null,
            digimon: digimonSlotDto.digimon ? DigimonConverter.convert(digimonSlotDto.digimon) : null
        };
    }
}
