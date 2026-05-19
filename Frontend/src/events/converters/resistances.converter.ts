import DigievolutionData from '../../database/Digievolution.json';
import { type Resistances, type Equipments, DigimonStatusType, type Equipament } from '../../models/Digimon';
import type { ResistancesDTO } from '../events.map';
import { DigimonStatusConverter } from '../../converters/DigimonStatusConverter';

export class ResistancesConverter {
    public static convert(
        baseResistances: ResistancesDTO | null,
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
            fire: DigimonStatusConverter.convert(DigimonStatusType.fire, baseResistances?.fire ?? 0, 'resistances', filteredEquipments, activeDigievolution),
            water: DigimonStatusConverter.convert(DigimonStatusType.water, baseResistances?.water ?? 0, 'resistances', filteredEquipments, activeDigievolution),
            ice: DigimonStatusConverter.convert(DigimonStatusType.ice, baseResistances?.ice ?? 0, 'resistances', filteredEquipments, activeDigievolution),
            wind: DigimonStatusConverter.convert(DigimonStatusType.wind, baseResistances?.wind ?? 0, 'resistances', filteredEquipments, activeDigievolution),
            thunder: DigimonStatusConverter.convert(DigimonStatusType.thunder, baseResistances?.thunder ?? 0, 'resistances', filteredEquipments, activeDigievolution),
            machine: DigimonStatusConverter.convert(DigimonStatusType.machine, baseResistances?.machine ?? 0, 'resistances', filteredEquipments, activeDigievolution),
            dark: DigimonStatusConverter.convert(DigimonStatusType.dark, baseResistances?.dark ?? 0, 'resistances', filteredEquipments, activeDigievolution),
        };
    }
}
