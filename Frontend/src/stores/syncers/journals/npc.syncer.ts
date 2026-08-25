import type { NpcBattleDTO } from "@/events/dto/npcs/npc-battle.dto";
import type { NpcDTO } from "@/events/dto/npcs/npc.dto";
import type { Npc, NpcBattle } from "@/models";

export class NpcSyncer {
  public static sync(previousNpc: Npc, newNpcDto: NpcDTO): void {
    if (newNpcDto.digimonBattles !== undefined) {
      newNpcDto.digimonBattles.forEach((newBattleDto) => {
        const previousBattle = previousNpc.digimonBattles.find((battle) => {
          return battle.id === newBattleDto.id;
        });

        if (previousBattle) {
          this.syncBattle(previousBattle, newBattleDto);
        }
      });
    }

    if (newNpcDto.cardBattles !== undefined) {
      newNpcDto.cardBattles.forEach((newBattleDto) => {
        const previousBattle = previousNpc.cardBattles.find((battle) => {
          return battle.id === newBattleDto.id;
        });

        if (previousBattle) {
          this.syncBattle(previousBattle, newBattleDto);
        }
      });
    }
  }

  private static syncBattle(previousBattle: NpcBattle, newBattleDto: NpcBattleDTO): void {
    if (newBattleDto.value !== undefined) {
      previousBattle.completed = newBattleDto.value !== 0;
    }
  }
}
