import type { DeepRequired } from "@/events/dto/deep-required";
import type { StoredDigievolutionDTO } from "@/events/dto/parties/digimons/stored-digievolution.dto";
import type { StoredDigievolution } from "@/models";

export class StoredDigievolutionConverter {
  public static convert(storedDigievolutionDto: DeepRequired<StoredDigievolutionDTO>): StoredDigievolution {
    return {
      digievolutionId: storedDigievolutionDto.digievolutionId,
      level: storedDigievolutionDto.level,
    };
  }
}
