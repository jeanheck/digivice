import type { Party } from "@/state/models";
import { PartyAssembler } from "@/memory/assemblers/party.assembler";
import { PartyReader } from "@/memory/readers/party.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class PartyProvider {
  public static get(snapshot: MemorySnapshot): Party {
    const resource = PartyReader.read(
      snapshot,
      AddressesRepository.getPartyAddresses(),
      AddressesRepository.getDigimonsAddresses(),
      AddressesRepository.getDigimonStatusAddresses(),
      AddressesRepository.getInBattleAddresses(),
    );
    return PartyAssembler.assemble(resource);
  }
}
