import type { Digimon } from '../../models';
import type { ResistancesDTO } from '../../events/dto/parties/digimons/resistances.dto';
import { ResistancesStateManager } from '../../stateManagers/ResistancesStateManager';

export class ResistancesSyncer {
    public static sync(previousDigimon: Digimon, resistancesDto: ResistancesDTO): void {
        ResistancesStateManager.refresh(previousDigimon, resistancesDto);
    }
}
