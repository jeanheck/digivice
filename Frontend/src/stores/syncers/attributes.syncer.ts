import type { Digimon } from '../../models';
import type { AttributesDTO } from '../../events/dto/parties/digimons/attributes.dto';
import { AttributesStateManager } from '../../stateManagers/AttributesStateManager';

export class AttributesSyncer {
    public static sync(previousDigimon: Digimon, attributesDto: AttributesDTO): void {
        AttributesStateManager.refresh(previousDigimon, attributesDto);
    }
}
