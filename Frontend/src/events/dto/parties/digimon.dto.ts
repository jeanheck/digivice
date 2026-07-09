import type { VitalsDTO } from './digimons/vitals.dto';
import type { AttributesDTO } from './digimons/attributes.dto';
import type { ResistancesDTO } from './digimons/resistances.dto';
import type { EquipmentsDTO } from './digimons/equipments.dto';
import type { DigievolutionSlotDTO } from './digimons/digievolution-slot.dto';
import type { StoredDigievolutionDTO } from './digimons/stored-digievolution.dto';

export interface DigimonDTO {
    level?: number;
    tp?: number;
    blastGauge?: number;
    experience?: number;
    vitals?: VitalsDTO;
    attributes?: AttributesDTO;
    resistances?: ResistancesDTO;
    equipments?: EquipmentsDTO;
    digievolutions?: DigievolutionSlotDTO[];
    storedDigievolutions?: StoredDigievolutionDTO[];
    activeDigievolutionId?: number;
}
