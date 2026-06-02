import type { Digimon } from './digimon/digimon';

export interface DigimonSlot {
    index: number;
    digimonId: number | null;
    digimon: Digimon | null;
}
