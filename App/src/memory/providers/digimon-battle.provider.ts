import type { DigimonBattle } from "@/state/models";
import { DigimonBattleAssembler } from "@/memory/assemblers/digimon-battle.assembler";
import { DigimonBattleReader } from "@/memory/readers/digimon-battle.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class DigimonBattleProvider {
  public static get(snapshot: MemorySnapshot): DigimonBattle {
    const resource = DigimonBattleReader.read(
      snapshot,
      AddressesRepository.getDigimonBattleAddresses(),
      AddressesRepository.getEnemyAddresses(),
    );
    return DigimonBattleAssembler.assemble(resource);
  }
}
