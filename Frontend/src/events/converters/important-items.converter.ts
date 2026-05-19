import type * as DTO from '../events.map';
import type { ImportantItems } from '../../models/Items';

export class ImportantItemsConverter {
    public static convert(importantItems: DTO.ImportantItemsDTO | null): ImportantItems | null {
        if (!importantItems) return null;
        return { ...importantItems };
    }
}
