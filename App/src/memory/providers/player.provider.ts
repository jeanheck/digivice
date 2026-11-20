import type { Player } from "@/state/models";
import { PlayerAssembler } from "@/memory/assemblers/player.assembler";
import { PlayerReader } from "@/memory/readers/player.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class PlayerProvider {
  public static get(snapshot: MemorySnapshot): Player {
    const resource = PlayerReader.read(snapshot, AddressesRepository.getPlayerAddresses());
    return PlayerAssembler.assemble(resource);
  }
}
