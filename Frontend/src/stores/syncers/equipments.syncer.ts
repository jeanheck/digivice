import type { Digimon } from '../../models';
import type { EquipmentsDTO } from '../../events/dto/parties/digimons/equipments.dto';
import { EquipmentsConverter } from '../../events/converters/equipments.converter';
import { AttributesStateManager } from '../../stateManagers/AttributesStateManager';
import { ResistancesStateManager } from '../../stateManagers/ResistancesStateManager';

export class EquipmentsSyncer {
    public static sync(previousDigimon: Digimon, equipmentsDto: EquipmentsDTO): void {
        previousDigimon.equipments = EquipmentsConverter.convert(equipmentsDto);
        AttributesStateManager.refresh(previousDigimon);
        ResistancesStateManager.refresh(previousDigimon);
    }
}
