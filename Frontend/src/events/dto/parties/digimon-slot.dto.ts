import type { DigimonDTO } from './digimon.dto';

export interface DigimonSlotDTO {
    index: number;
    digimonId?: number | null;
    digimon?: DigimonDTO | null;
}
