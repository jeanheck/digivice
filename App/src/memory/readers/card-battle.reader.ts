import type { CardBattleAddresses } from "@/memory/addresses/card-battle.addresses";
import type { CardBattleResource } from "@/memory/resources/card-battle.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class CardBattleReader {
  public static read(snapshot: MemorySnapshot, addresses: CardBattleAddresses): CardBattleResource {
    return {
      id: snapshot.readInt32(addresses.id),
    };
  }
}
