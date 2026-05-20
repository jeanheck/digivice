import type { Digimon } from './digimon';

export interface DigimonSlot {
    index: number;
    digimonId: number | null;
    digimon: Digimon | null;
}
