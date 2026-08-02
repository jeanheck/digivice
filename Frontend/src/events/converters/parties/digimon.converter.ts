import type { Digimon } from '@/models';
import type { DigimonDTO } from '@/events/dto/parties/digimon.dto';
import { VitalConverter } from './digimons/vital.converter';
import { AttributesConverter } from './digimons/attributes.converter';
import { ResistancesConverter } from './digimons/resistances.converter';
import { EquipmentsConverter } from './digimons/equipments.converter';
import { DigievolutionSlotConverter } from './digimons/digievolution-slot.converter';
import { StoredDigievolutionConverter } from './digimons/stored-digievolution.converter';

export class DigimonConverter {
    public static convert(digimonDto: DigimonDTO): Digimon {
        return {
            level: digimonDto.level ?? 1,
            tp: digimonDto.tp ?? 0,
            blast: digimonDto.blast ?? 0,
            experience: digimonDto.experience ?? 0,
            activeDigievolutionId: digimonDto.activeDigievolutionId ?? null,
            hp: VitalConverter.convert(digimonDto.hp ?? null),
            mp: VitalConverter.convert(digimonDto.mp ?? null),
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
