import type { VitalDTO } from "@/events/dto/parties/digimons/vital.dto";
import type { Vital } from "@/models/party/digimon/vital";

export class VitalConverter {
  public static convert(newVitalDto: VitalDTO | null): Vital {
    return {
      current: newVitalDto?.current ?? 0,
      max: newVitalDto?.max ?? 0,
    };
  }
}
