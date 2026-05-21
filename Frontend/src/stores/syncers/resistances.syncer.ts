import type { Resistances } from '../../models';
import type { ResistancesDTO } from '../../events/dto/parties/digimons/resistances.dto';

export class ResistancesSyncer {
    public static sync(previousResistances: Resistances, newResistancesDto: ResistancesDTO): void {
        if (newResistancesDto.fire !== undefined) {
            previousResistances.fire = newResistancesDto.fire;
        }
        if (newResistancesDto.water !== undefined) {
            previousResistances.water = newResistancesDto.water;
        }
        if (newResistancesDto.ice !== undefined) {
            previousResistances.ice = newResistancesDto.ice;
        }
        if (newResistancesDto.wind !== undefined) {
            previousResistances.wind = newResistancesDto.wind;
        }
        if (newResistancesDto.thunder !== undefined) {
            previousResistances.thunder = newResistancesDto.thunder;
        }
        if (newResistancesDto.machine !== undefined) {
            previousResistances.machine = newResistancesDto.machine;
        }
        if (newResistancesDto.dark !== undefined) {
            previousResistances.dark = newResistancesDto.dark;
        }
    }
}
