import type { ImportantItems } from "@/state/models";
import { ImportantItemsAssembler } from "@/memory/assemblers/important-items.assembler";
import { ImportantItemsReader } from "@/memory/readers/important-items.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class ImportantItemsProvider {
  public static get(snapshot: MemorySnapshot): ImportantItems {
    const resource = ImportantItemsReader.read(
      snapshot,
      AddressesRepository.getImportantItemsAddresses(),
    );
    return ImportantItemsAssembler.assemble(resource);
  }
}
