import type { Equipament } from './equipament';

export interface Equipments {
    head: Equipament | null;
    body: Equipament | null;
    rightHand: Equipament | null;
    leftHand: Equipament | null;
    accessory1: Equipament | null;
    accessory2: Equipament | null;
}
