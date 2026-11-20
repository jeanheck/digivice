import type { DigimonSlot, Party } from "@/state/models";
import { DigimonAssembler } from "@/memory/assemblers/digimon.assembler";
import type { PartyResource } from "@/memory/resources/party.resource";
import type { DigimonSlotResource } from "@/memory/resources/parties/digimon-slot.resource";

export class PartyAssembler {
  private static assembleSlot(resource: DigimonSlotResource): DigimonSlot {
    if (resource.digimon === null) {
      return { index: resource.index, digimonId: null, digimon: null };
    }

    return {
      index: resource.index,
      digimonId: resource.digimonId,
      digimon: DigimonAssembler.assemble(resource.digimon),
    };
  }

  public static assemble(resource: PartyResource): Party {
    return {
      slots: resource.slots.map((slot) => PartyAssembler.assembleSlot(slot)),
    };
  }
}
