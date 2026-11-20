import type { CardBattle } from "@/state/models";
import { CardBattleAssembler } from "@/memory/assemblers/card-battle.assembler";
import { CardBattleReader } from "@/memory/readers/card-battle.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class CardBattleProvider {
  public static get(snapshot: MemorySnapshot): CardBattle {
    const resource = CardBattleReader.read(snapshot, AddressesRepository.getCardBattleAddresses());
    return CardBattleAssembler.assemble(resource);
  }
}
