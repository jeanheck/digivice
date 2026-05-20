import type { Digimon } from './Digimon';

export interface DigimonSlot {
    index: number;
    digimonId: number | null;
    digimon: Digimon | null;
}

export interface Party {
    slots: DigimonSlot[];
    groupCharisma: number;
}
