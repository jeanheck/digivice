import LocationsData from '../database/Locations.json';
import type { Location } from '../models/Location';

export class LocationCalculator {
    public static getFromMapId(mapId: string): Location | null {
        if (!mapId) return null;

        const data = (LocationsData as Record<string, any>)[mapId.toUpperCase()];
        if (!data) return null;

        return {
            mapId: mapId,
            location: data.Location,
            name: data.Name,
            image: data.Image
        };
    }
}
