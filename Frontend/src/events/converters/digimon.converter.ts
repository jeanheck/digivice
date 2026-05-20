import type * as DTO from '../events.map';
import type * as Model from '../../models/Digimon';
import { AttributesConverter } from './attributes.converter';
import { ResistancesConverter } from './resistances.converter';
import { EquipmentsConverter } from './equipments.converter';
import { DigievolutionsConverter } from './digievolutions.converter';

export class DigimonConverter {
    public static convert(digimon: DTO.DigimonDTO | null): Model.Digimon | null {
        if (!digimon) return null;

        const level = digimon.level ?? 1;
        const experience = digimon.experience ?? 0;

        const vitals: Model.Vitals = {
            currentHP: digimon.vitals?.currentHP ?? 0,
            maxHP: digimon.vitals?.maxHP ?? 0,
            currentMP: digimon.vitals?.currentMP ?? 0,
            maxMP: digimon.vitals?.maxMP ?? 0
        };

        const equipments = EquipmentsConverter.convert(digimon.equipments ?? null);
        const activeDigievolutionId = digimon.activeDigievolutionId ?? null;

        return {
            vitals,
            level,
            experience,
            attributes: AttributesConverter.convert(digimon.attributes ?? null, equipments, activeDigievolutionId),
            resistances: ResistancesConverter.convert(digimon.resistances ?? null, equipments, activeDigievolutionId),
            equipments,
            activeDigievolutionId,
            digievolutions: DigievolutionsConverter.convert(digimon.digievolutions ?? null)
        };
    }
}
