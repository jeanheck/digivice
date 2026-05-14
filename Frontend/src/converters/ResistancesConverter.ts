import EquipmentsData from '../database/Equipments.json';
import DigievolutionData from '../database/Digievolution.json';
import type { Resistances, Equipments } from '../models/Digimon';
import type { ResistancesDTO } from '../dtos/events.dto';
import { DigimonStatusConverter } from './DigimonStatusConverter';

export class ResistancesConverter {
    public static convert(
        baseResistances: ResistancesDTO,
        equipments: Equipments | Record<string, number>,
        activeDigievolutionId: number | null
    ): Resistances {
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
            fire: DigimonStatusConverter.convert('fire', baseResistances.fire, 'resistances', filteredEquipments, activeDigievolution),
            water: DigimonStatusConverter.convert('water', baseResistances.water, 'resistances', filteredEquipments, activeDigievolution),
            ice: DigimonStatusConverter.convert('ice', baseResistances.ice, 'resistances', filteredEquipments, activeDigievolution),
            wind: DigimonStatusConverter.convert('wind', baseResistances.wind, 'resistances', filteredEquipments, activeDigievolution),
            thunder: DigimonStatusConverter.convert('thunder', baseResistances.thunder, 'resistances', filteredEquipments, activeDigievolution),
            machine: DigimonStatusConverter.convert('machine', baseResistances.machine, 'resistances', filteredEquipments, activeDigievolution),
            dark: DigimonStatusConverter.convert('dark', baseResistances.dark, 'resistances', filteredEquipments, activeDigievolution),
        };
    }
}
