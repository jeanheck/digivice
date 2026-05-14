import DigievolutionData from '../database/Digievolution.json';
import { type Resistances, type Equipments, DigimonStatusType, type Equipament } from '../models/Digimon';
import type { ResistancesDTO } from '../dtos/events.dto';
import { DigimonStatusConverter } from './DigimonStatusConverter';

export class ResistancesConverter {
    public static convert(
        baseResistances: ResistancesDTO,
        equipments: Equipments,
        activeDigievolutionId: number | null
    ): Resistances {
        const filteredEquipments = Object.values(equipments).filter(e => e !== null) as Equipament[];

        let activeDigievolution = null;
        if (activeDigievolutionId) {
            activeDigievolution = DigievolutionData.digievolutions
                .find(d => d.id === activeDigievolutionId) || null;
        }

        return {
            fire: DigimonStatusConverter.convert(DigimonStatusType.fire, baseResistances.fire, 'resistances', filteredEquipments, activeDigievolution),
            water: DigimonStatusConverter.convert(DigimonStatusType.water, baseResistances.water, 'resistances', filteredEquipments, activeDigievolution),
            ice: DigimonStatusConverter.convert(DigimonStatusType.ice, baseResistances.ice, 'resistances', filteredEquipments, activeDigievolution),
            wind: DigimonStatusConverter.convert(DigimonStatusType.wind, baseResistances.wind, 'resistances', filteredEquipments, activeDigievolution),
            thunder: DigimonStatusConverter.convert(DigimonStatusType.thunder, baseResistances.thunder, 'resistances', filteredEquipments, activeDigievolution),
            machine: DigimonStatusConverter.convert(DigimonStatusType.machine, baseResistances.machine, 'resistances', filteredEquipments, activeDigievolution),
            dark: DigimonStatusConverter.convert(DigimonStatusType.dark, baseResistances.dark, 'resistances', filteredEquipments, activeDigievolution),
        };
    }
}
