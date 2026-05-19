import type { DigimonSlotDTO } from '../dto/parties/digimon-slot.dto';
import type { Digimon } from '../../models/Digimon';
import { DigimonConverter } from './digimon.converter';

export class DigimonSlotConverter {
    public static convert(slotDto: DigimonSlotDTO | null): Digimon | null {
        if (!slotDto || !slotDto.digimon) return null;
        return DigimonConverter.convert(slotDto.digimon, slotDto.index);
    }
}
