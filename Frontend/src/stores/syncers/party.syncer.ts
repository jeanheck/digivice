import type { Party } from "@/models";
import type { PartyDTO } from "@/events/dto/party.dto";
import { DigimonSlotSyncer } from "./parties/digimon-slot.syncer";

export class PartySyncer {
  public static sync(previousParty: Party, newPartyDto: PartyDTO): void {
    if (newPartyDto.slots) {
      newPartyDto.slots.forEach((newSlotDto) => {
        const previousSlot = previousParty.slots.find((slot) => slot.index === newSlotDto.index);
        if (previousSlot) {
          DigimonSlotSyncer.sync(previousSlot, newSlotDto);
        }
      });
    }
  }
}
