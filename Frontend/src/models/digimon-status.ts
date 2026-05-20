import type { DigimonStatusType } from './digimon-status-type';

export interface DigimonStatus {
    digimonStatusType: DigimonStatusType;
    fromDigimon: number;
    fromEquipaments: number;
    fromDigievolution: number;
    sumBetweenDigimonAndEquipaments: number;
}
