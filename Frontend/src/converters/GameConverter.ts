import type * as DTO from '../dtos/events.dto';
import type { Player, Party } from '../models/Player';
import type { ImportantItems, ConsumableItems } from '../models/Items';

import { PartySlotsConverter } from './PartySlotsConverter';

/**
 * GameConverter
 * Responsible for transforming raw DTOs from the backend into rich Domain Models for the UI.
 */
export class GameConverter {

    public static toPlayerModel(dto: DTO.PlayerDTO | null): Player | null {
        if (!dto) return null;
        return { ...dto };
    }



    public static toPartyModel(dto: DTO.PartyDTO | null): Party | null {
        if (!dto) return null;
        return {
            slots: PartySlotsConverter.convert(dto.slots)
        };
    }



    public static toConsumableItemsModel(dto: DTO.ConsumableItemsDTO | null): ConsumableItems | null {
        if (!dto) return null;
        return { ...dto };
    }

}
