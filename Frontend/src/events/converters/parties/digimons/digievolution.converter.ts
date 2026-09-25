import type { DeepRequired } from "@/events/dto/deep-required";
import type { DigievolutionDTO } from "@/events/dto/parties/digimons/digievolution.dto";
import type { Digievolution } from "@/models";

export class DigievolutionConverter {
  public static convert(digievolutionDto: DeepRequired<DigievolutionDTO>): Digievolution {
    return {
      level: digievolutionDto.level,
      dvexp: digievolutionDto.dvexp,
    };
  }
}
