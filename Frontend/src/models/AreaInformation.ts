import type { Enemy } from './Enemy';
import type { Location } from './Location';

export interface AreaInformation {
    location: Location;
    enemies: Enemy[];
}
