import type { DigimonSlotDTO } from '../events/dto/Parties/DigimonSlotDTO';
import type { Digimon } from '../models/Digimon';
import { DigimonConverter } from './DigimonConverter';

export class DigimonSlotConverter {
    public static convert(slotDto: DigimonSlotDTO | null): Digimon | null {
        if (!slotDto || !slotDto.digimon) return null;
        return DigimonConverter.convert(slotDto.digimon, slotDto.index);
    }
}
