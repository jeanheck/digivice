import type { DigimonDTO } from './DigimonDTO';

export interface DigimonSlotDTO {
    index: number;
    digimonId?: number | null;
    digimon?: DigimonDTO | null;
}
