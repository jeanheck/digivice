import type { Vital } from "@/models";
import type { VitalDTO } from "@/events/dto/parties/digimons/vital.dto";

export class VitalSyncer {
    public static sync(previousVital: Vital, newVitalDto: VitalDTO): void {
        if (newVitalDto.current !== undefined) {
            previousVital.current = newVitalDto.current;
        }
        if (newVitalDto.max !== undefined) {
            previousVital.max = newVitalDto.max;
        }
    }
}
