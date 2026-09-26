import type { DeepRequired } from "@/events/dto/deep-required";
import type { VitalDTO } from "@/events/dto/parties/digimons/vital.dto";
import type { Vital } from "@/models";

export class VitalConverter {
  public static convert(vitalDto: DeepRequired<VitalDTO>): Vital {
    return {
      current: vitalDto.current,
      max: vitalDto.max,
    };
  }
}
