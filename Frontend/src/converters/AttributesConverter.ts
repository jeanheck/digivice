import EquipmentsData from '../database/Equipments.json';
import DigievolutionData from '../database/Digievolution.json';
import type { Attributes, Equipments } from '../models/Digimon';
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
            strength: DigimonStatusConverter.convert('strength', baseAttributes.strength, 'attributes', filteredEquipments, activeDigievolution),
            defense: DigimonStatusConverter.convert('defense', baseAttributes.defense, 'attributes', filteredEquipments, activeDigievolution),
            spirit: DigimonStatusConverter.convert('spirit', baseAttributes.spirit, 'attributes', filteredEquipments, activeDigievolution),
            wisdom: DigimonStatusConverter.convert('wisdom', baseAttributes.wisdom, 'attributes', filteredEquipments, activeDigievolution),
            speed: DigimonStatusConverter.convert('speed', baseAttributes.speed, 'attributes', filteredEquipments, activeDigievolution),
            charisma: DigimonStatusConverter.convert('charisma', baseAttributes.charisma, 'attributes', filteredEquipments, activeDigievolution),
        };
    }
}
