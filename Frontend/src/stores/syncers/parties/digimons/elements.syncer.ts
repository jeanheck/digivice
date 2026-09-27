import type { Elements } from "@/models";
import type { ElementsDTO } from "@/events/dto/parties/digimons/elements.dto";

export class ElementsSyncer {
  public static sync(previousElements: Elements, newElementsDto: ElementsDTO): void {
    if (newElementsDto.fire !== undefined) {
      previousElements.fire = newElementsDto.fire;
    }
    if (newElementsDto.water !== undefined) {
      previousElements.water = newElementsDto.water;
    }
    if (newElementsDto.ice !== undefined) {
      previousElements.ice = newElementsDto.ice;
    }
    if (newElementsDto.wind !== undefined) {
      previousElements.wind = newElementsDto.wind;
    }
    if (newElementsDto.thunder !== undefined) {
      previousElements.thunder = newElementsDto.thunder;
    }
    if (newElementsDto.machine !== undefined) {
      previousElements.machine = newElementsDto.machine;
    }
    if (newElementsDto.dark !== undefined) {
      previousElements.dark = newElementsDto.dark;
    }
  }
}
