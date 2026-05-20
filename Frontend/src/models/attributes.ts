import type { DigimonStatus } from './digimon-status';

export interface Attributes {
    strength: DigimonStatus;
    defense: DigimonStatus;
    spirit: DigimonStatus;
    wisdom: DigimonStatus;
    speed: DigimonStatus;
    charisma: DigimonStatus;
}
