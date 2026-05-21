import type { EquipmentsDTO } from "@/events/dto/parties/digimons/equipments.dto";
import type { Equipments } from "@/models";

export class EquipmentsConverter {
    public static convert(equipmentsDto: EquipmentsDTO | null): Equipments {
        return {
            head: equipmentsDto?.head ?? null,
            body: equipmentsDto?.body ?? null,
            rightHand: equipmentsDto?.rightHand ?? null,
            leftHand: equipmentsDto?.leftHand ?? null,
            accessory1: equipmentsDto?.accessory1 ?? null,
            accessory2: equipmentsDto?.accessory2 ?? null
        };
    }
}
