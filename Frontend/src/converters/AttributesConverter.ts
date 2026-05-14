import EquipmentsData from '../database/Equipments.json';
import DigievolutionData from '../database/Digievolution.json';
import { DigimonStatusType, type Attributes, type Equipments } from '../models/Digimon';
import type { AttributesDTO } from '../dtos/events.dto';
import { DigimonStatusConverter } from './DigimonStatusConverter';

export class AttributesConverter {
    public static convert(
        baseAttributes: AttributesDTO,
        equipments: Equipments | Record<string, number>,
        activeDigievolutionId: number | null
    ): Attributes {
        const equipmentsIds = Object.values(equipments).filter(id => id > 0);
        const filteredEquipments = equipmentsIds
            .map(id => EquipmentsData.equipments.find(e => e.Id === id))
            .filter(i => i);

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
