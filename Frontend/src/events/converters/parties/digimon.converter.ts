import type { Digimon } from '@/models';
import type { DigimonDTO } from '@/events/dto/parties/digimon.dto';
import { VitalsConverter } from './digimons/vitals.converter';
import { AttributesConverter } from './digimons/attributes.converter';
import { ResistancesConverter } from './digimons/resistances.converter';
import { EquipmentsConverter } from './digimons/equipments.converter';
import { DigievolutionSlotConverter } from './digimons/digievolution-slot.converter';
import { StoredDigievolutionConverter } from './digimons/stored-digievolution.converter';

export class DigimonConverter {
    public static convert(digimonDto: DigimonDTO): Digimon {
        return {
            level: digimonDto.level ?? 1,
            experience: digimonDto.experience ?? 0,
            activeDigievolutionId: digimonDto.activeDigievolutionId ?? null,
            vitals: VitalsConverter.convert(digimonDto.vitals ?? null),
            attributes: AttributesConverter.convert(digimonDto.attributes ?? null),
            resistances: ResistancesConverter.convert(digimonDto.resistances ?? null),
            equipments: EquipmentsConverter.convert(digimonDto.equipments ?? null),
            digievolutions: digimonDto.digievolutions
                ? digimonDto.digievolutions.map(slot => DigievolutionSlotConverter.convert(slot))
                : [],
            storedDigievolutions: digimonDto.storedDigievolutions
                ? digimonDto.storedDigievolutions.map(stored => StoredDigievolutionConverter.convert(stored))
                : []
        };
    }
}
