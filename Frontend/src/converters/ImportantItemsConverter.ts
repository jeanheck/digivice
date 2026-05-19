import type * as DTO from '../events/dto/events.dto';
import type { ImportantItems } from '../models/Items';

export class ImportantItemsConverter {
    public static convert(importantItems: DTO.ImportantItemsDTO | null): ImportantItems | null {
        if (!importantItems) return null;
        return { ...importantItems };
    }
}
