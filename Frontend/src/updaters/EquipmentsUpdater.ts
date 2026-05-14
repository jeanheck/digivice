import type { Digimon, Equipments } from '../models/Digimon';

export class EquipmentsUpdater {
    public static update(digimon: Digimon, newEquipments: Equipments): void {
        digimon.equipments = newEquipments;
    }
}
