import type { VitalsDTO } from './Digimons/VitalsDTO';
import type { AttributesDTO } from './Digimons/AttributesDTO';
import type { ResistancesDTO } from './Digimons/ResistancesDTO';
import type { EquipmentsDTO } from './Digimons/EquipmentsDTO';
import type { DigievolutionSlotDTO } from './Digimons/DigievolutionSlotDTO';

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
