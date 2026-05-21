import DigievolutionData from '../../../../database/Digievolution.json';
import { DigimonStatusType, type Attributes, type Equipments, type Equipament } from '../../../../models';
import type { AttributesDTO } from '../../../events.map';
import { DigimonStatusConverter } from '../../../../converters/DigimonStatusConverter';

export class AttributesConverterBackup {
    public static convert(
        baseAttributes: AttributesDTO | null,
        equipments: Equipments,
        activeDigievolutionId: number | null
    ): Attributes {
        const filteredEquipments = Object.values(equipments).filter(e => e !== null) as Equipament[];

        let activeDigievolution = null;
        if (activeDigievolutionId) {
            activeDigievolution = DigievolutionData.digievolutions
                .find(d => d.id === activeDigievolutionId) || null;
        }

        return {
            strength: DigimonStatusConverter.convert(DigimonStatusType.strength, baseAttributes?.strength ?? 0, 'attributes', filteredEquipments, activeDigievolution),
            defense: DigimonStatusConverter.convert(DigimonStatusType.defense, baseAttributes?.defense ?? 0, 'attributes', filteredEquipments, activeDigievolution),
            spirit: DigimonStatusConverter.convert(DigimonStatusType.spirit, baseAttributes?.spirit ?? 0, 'attributes', filteredEquipments, activeDigievolution),
            wisdom: DigimonStatusConverter.convert(DigimonStatusType.wisdom, baseAttributes?.wisdom ?? 0, 'attributes', filteredEquipments, activeDigievolution),
            speed: DigimonStatusConverter.convert(DigimonStatusType.speed, baseAttributes?.speed ?? 0, 'attributes', filteredEquipments, activeDigievolution),
            charisma: DigimonStatusConverter.convert(DigimonStatusType.charisma, baseAttributes?.charisma ?? 0, 'attributes', filteredEquipments, activeDigievolution),
        };
    }
}
