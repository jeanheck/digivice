import type { ImportantItemsAddresses } from "@/memory/addresses/important-items.addresses";
import type { ImportantItemsResource } from "@/memory/resources/important-items.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class ImportantItemsReader {
  public static read(
    snapshot: MemorySnapshot,
    addresses: ImportantItemsAddresses,
  ): ImportantItemsResource {
    return {
      treeBoots: snapshot.readByte(addresses.treeBoots),
      fishingPole: snapshot.readByte(addresses.fishingPole),
      asukaTrophy: snapshot.readByte(addresses.asukaTrophy),
      sunTrophy: snapshot.readByte(addresses.sunTrophy),
    };
  }
}
