import type { DeepRequired } from "@/events/dto/deep-required";
import type { ResistancesDTO } from "@/events/dto/parties/digimons/resistances.dto";
import type { Resistances } from "@/models";

export class ResistancesConverter {
  public static convert(resistancesDto: DeepRequired<ResistancesDTO>): Resistances {
    return {
      fire: resistancesDto.fire,
      water: resistancesDto.water,
      ice: resistancesDto.ice,
      wind: resistancesDto.wind,
      thunder: resistancesDto.thunder,
      machine: resistancesDto.machine,
      dark: resistancesDto.dark,
    };
  }
}
