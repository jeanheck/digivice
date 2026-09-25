import type { DeepRequired } from "@/events/dto/deep-required";
import type { DigimonSlotDTO } from "@/events/dto/parties/digimon-slot.dto";
import type { DigimonSlot } from "@/models";
import { DigimonConverter } from "./digimon.converter";

export class DigimonSlotConverter {
  public static convert(digimonSlotDto: DeepRequired<DigimonSlotDTO>): DigimonSlot {
    return {
      index: digimonSlotDto.index,
      digimonId: digimonSlotDto.digimonId,
      digimon: digimonSlotDto.digimon ? DigimonConverter.convert(digimonSlotDto.digimon) : null,
    };
  }
}
