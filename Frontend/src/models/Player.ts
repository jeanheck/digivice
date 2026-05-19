import type { Digimon } from './Digimon';

export interface Player {
    name: string;
    bits: number;
    location: string;
}

export interface Party {
    slots: (Digimon | null)[];
    groupCharisma: number;
}
