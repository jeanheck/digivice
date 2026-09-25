import type { DeepRequired } from "@/events/dto/deep-required";
import type { PartyDTO } from "@/events/dto/party.dto";
import type { Party } from "@/models";
import { DigimonSlotConverter } from "./parties/digimon-slot.converter";

export class PartyConverter {
  public static convert(partyDto: DeepRequired<PartyDTO>): Party {
    return {
      slots: partyDto.slots.map((digimonSlotDto) => DigimonSlotConverter.convert(digimonSlotDto)),
    };
  }
}
