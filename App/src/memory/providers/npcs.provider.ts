import type { Npcs } from "@/state/models";
import { NpcsAssembler } from "@/memory/assemblers/npcs.assembler";
import { NpcsReader } from "@/memory/readers/npcs.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class NpcsProvider {
  public static get(snapshot: MemorySnapshot): Npcs {
    const resource = NpcsReader.read(snapshot, AddressesRepository.getNpcsAddresses());
    return NpcsAssembler.assemble(resource);
  }
}
