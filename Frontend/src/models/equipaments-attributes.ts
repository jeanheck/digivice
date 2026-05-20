import type { DigimonStatusType } from './digimon-status-type';
import type { EquipamentsAttributesOperationType } from './equipaments-attributes-operation-type';

export interface EquipamentsAttributes {
    attribute: DigimonStatusType;
    type: EquipamentsAttributesOperationType;
    value: number;
}
