import type { DigimonBattleAddresses } from "@/memory/addresses/digimon-battle.addresses";
import type { EnemyAddresses } from "@/memory/addresses/enemy.addresses";
import { EnemyReader } from "@/memory/readers/enemy.reader";
import type { DigimonBattleResource } from "@/memory/resources/digimon-battle.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class DigimonBattleReader {
  public static read(
    snapshot: MemorySnapshot,
    digimonBattleAddresses: DigimonBattleAddresses,
    enemyAddresses: EnemyAddresses,
  ): DigimonBattleResource {
    return {
      field: snapshot.readByte(digimonBattleAddresses.field),
      enemy: EnemyReader.read(snapshot, enemyAddresses),
    };
  }
}
