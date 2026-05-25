import type { LocationTable } from './tables/location/location-table';
import LocationJson from '@/database/location.json';
import type { LocationRaw } from './tables/raws/location-raw';

export class LocationRepository {
    private static readonly locationTable = LocationJson as LocationTable;

    public static getLocationById(id: string): LocationRaw {
        return this.locationTable[id]!;
    }
}
