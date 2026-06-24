import type { VitalsDTO } from "@/events/dto/parties/digimons/vitals.dto";
import type { Vitals } from "@/models/party/digimon/vitals";

export class VitalsConverter {
    public static convert(newVitalsDto: VitalsDTO | null): Vitals {
        return {
            currentHP: newVitalsDto?.currentHP ?? 0,
            maxHP: newVitalsDto?.maxHP ?? 0,
            currentMP: newVitalsDto?.currentMP ?? 0,
            maxMP: newVitalsDto?.maxMP ?? 0
        };
    }
}