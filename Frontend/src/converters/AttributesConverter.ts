import DigievolutionData from '../database/Digievolution.json';
import { DigimonStatusType, type Attributes, type Equipments, type Equipament } from '../models/Digimon';
import type { AttributesDTO } from '../dtos/events.dto';
import { DigimonStatusConverter } from './DigimonStatusConverter';

export class AttributesConverter {
    public static convert(
        baseAttributes: AttributesDTO,
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
            strength: DigimonStatusConverter.convert(DigimonStatusType.strength, baseAttributes.strength, 'attributes', filteredEquipments, activeDigievolution),
            defense: DigimonStatusConverter.convert(DigimonStatusType.defense, baseAttributes.defense, 'attributes', filteredEquipments, activeDigievolution),
            spirit: DigimonStatusConverter.convert(DigimonStatusType.spirit, baseAttributes.spirit, 'attributes', filteredEquipments, activeDigievolution),
            wisdom: DigimonStatusConverter.convert(DigimonStatusType.wisdom, baseAttributes.wisdom, 'attributes', filteredEquipments, activeDigievolution),
            speed: DigimonStatusConverter.convert(DigimonStatusType.speed, baseAttributes.speed, 'attributes', filteredEquipments, activeDigievolution),
            charisma: DigimonStatusConverter.convert(DigimonStatusType.charisma, baseAttributes.charisma, 'attributes', filteredEquipments, activeDigievolution),
        };
    }
}
