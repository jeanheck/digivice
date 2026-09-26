import type { DeepRequired } from "@/events/dto/deep-required";
import type { AttributesDTO } from "@/events/dto/parties/digimons/attributes.dto";
import type { Attributes } from "@/models";

export class AttributesConverter {
  public static convert(attributesDto: DeepRequired<AttributesDTO>): Attributes {
    return {
      strength: attributesDto.strength,
      defense: attributesDto.defense,
      spirit: attributesDto.spirit,
      wisdom: attributesDto.wisdom,
      speed: attributesDto.speed,
      charisma: attributesDto.charisma,
    };
  }
}
