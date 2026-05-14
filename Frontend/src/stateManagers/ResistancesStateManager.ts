import type { Digimon } from '../models/Digimon';
import type { ResistancesDTO } from '../dtos/events.dto';
import { ResistancesConverter } from '../converters/ResistancesConverter';
import { ResistancesUpdater } from '../updaters/ResistancesUpdater';

export class ResistancesStateManager {
    public static refresh(digimon: Digimon, newBaseResistances?: ResistancesDTO): void {
        const baseResistances: ResistancesDTO = newBaseResistances || {
            fire: digimon.resistances.fire.fromDigimon,
            water: digimon.resistances.water.fromDigimon,
            ice: digimon.resistances.ice.fromDigimon,
            wind: digimon.resistances.wind.fromDigimon,
            thunder: digimon.resistances.thunder.fromDigimon,
            machine: digimon.resistances.machine.fromDigimon,
            dark: digimon.resistances.dark.fromDigimon
        };

        const newResistances = ResistancesConverter.convert(
            baseResistances, 
            digimon.equipments, 
            digimon.activeDigievolutionId
        );

        ResistancesUpdater.update(digimon, newResistances);
    }
}
