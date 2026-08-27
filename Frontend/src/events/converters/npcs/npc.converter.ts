import type { NpcBattleDTO } from "@/events/dto/npcs/npc-battle.dto";
import type { NpcDTO } from "@/events/dto/npcs/npc.dto";
import type { Npc, NpcBattle } from "@/models";

export class NpcConverter {
  public static convert(npcDto: NpcDTO): Npc {
    return {
      id: npcDto.id,
      digimonBattles: (npcDto.digimonBattles ?? []).map((battleDto) => {
        return this.convertBattle(battleDto);
      }),
    };
  }

  public static convertBattle(battleDto: NpcBattleDTO): NpcBattle {
    return {
      id: battleDto.id,
      completed: battleDto.value !== undefined && battleDto.value !== 0,
    };
  }
}
