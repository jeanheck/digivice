import type { Enemy } from './enemy';
import type { Location } from './location';

export interface AreaInformation {
    location: Location;
    enemies: Enemy[];
}
