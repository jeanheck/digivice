import type { DeepRequired } from "@/events/dto/deep-required";
import type { ElementsDTO } from "@/events/dto/parties/digimons/elements.dto";
import type { Elements } from "@/models";

export class ElementsConverter {
  public static convert(elementsDto: DeepRequired<ElementsDTO>): Elements {
    return {
      fire: elementsDto.fire,
      water: elementsDto.water,
      ice: elementsDto.ice,
      wind: elementsDto.wind,
      thunder: elementsDto.thunder,
      machine: elementsDto.machine,
      dark: elementsDto.dark,
    };
  }
}
