import type { DigimonStatusType } from './digimon-status-type';
import type { EquipmentsAttributesOperationType as EquipmentsAttributesOperationType } from './equipments-attributes-operation-type';

export interface EquipmentsAttributes {
    attribute: DigimonStatusType;
    type: EquipmentsAttributesOperationType;
    value: number;
}
