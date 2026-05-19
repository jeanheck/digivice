import type { VitalsDTO } from './digimons/vitals.dto';
import type { AttributesDTO } from './digimons/attributes.dto';
import type { ResistancesDTO } from './digimons/resistances.dto';
import type { EquipmentsDTO } from './digimons/equipments.dto';
import type { DigievolutionSlotDTO } from './digimons/digievolution-slot.dto';

export interface DigimonDTO {
    level?: number;
    experience?: number;
    vitals?: VitalsDTO;
    attributes?: AttributesDTO;
    resistances?: ResistancesDTO;
    equipments?: EquipmentsDTO;
    digievolutions?: DigievolutionSlotDTO[];
    activeDigievolutionId?: number;
}
