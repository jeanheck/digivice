import type { DigimonStatusType } from './digimon-status-type';

export interface EquipmentsAttributes {
    attribute: DigimonStatusType;
    type: string;
    value: number;
}
