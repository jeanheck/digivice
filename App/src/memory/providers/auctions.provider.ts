import type { Auctions } from "@/state/models";
import { AuctionsAssembler } from "@/memory/assemblers/auctions.assembler";
import { AuctionsReader } from "@/memory/readers/auctions.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class AuctionsProvider {
  public static get(snapshot: MemorySnapshot): Auctions {
    const resource = AuctionsReader.read(snapshot, AddressesRepository.getAuctionsAddresses());
    return AuctionsAssembler.assemble(resource);
  }
}
