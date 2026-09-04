import type { NpcBattleDTO } from "@/events/dto/npcs/npc-battle.dto";
import type { NpcDTO } from "@/events/dto/npcs/npc.dto";
import type { NpcsDTO } from "@/events/dto/npcs.dto";
import type { Npc, NpcBattle, Npcs } from "@/models";

export class NpcsSyncer {
  public static sync(previousNpcs: Npcs, newNpcsDto: NpcsDTO): void {
    this.syncNpcIfPresent(previousNpcs.genji, newNpcsDto.genji);
    this.syncNpcIfPresent(previousNpcs.natsumi, newNpcsDto.natsumi);
    this.syncNpcIfPresent(previousNpcs.catherine, newNpcsDto.catherine);
    this.syncNpcIfPresent(previousNpcs.lucia, newNpcsDto.lucia);
    this.syncNpcIfPresent(previousNpcs.robert, newNpcsDto.robert);
    this.syncNpcIfPresent(previousNpcs.akiba, newNpcsDto.akiba);
    this.syncNpcIfPresent(previousNpcs.chris, newNpcsDto.chris);
    this.syncNpcIfPresent(previousNpcs.tomomi, newNpcsDto.tomomi);
    this.syncNpcIfPresent(previousNpcs.mitch, newNpcsDto.mitch);
    this.syncNpcIfPresent(previousNpcs.bob, newNpcsDto.bob);
    this.syncNpcIfPresent(previousNpcs.andy, newNpcsDto.andy);
    this.syncNpcIfPresent(previousNpcs.george, newNpcsDto.george);
    this.syncNpcIfPresent(previousNpcs.meiLin, newNpcsDto.meiLin);
    this.syncNpcIfPresent(previousNpcs.jessica, newNpcsDto.jessica);
    this.syncNpcIfPresent(previousNpcs.gordon, newNpcsDto.gordon);
    this.syncNpcIfPresent(previousNpcs.alice, newNpcsDto.alice);
    this.syncNpcIfPresent(previousNpcs.nakano, newNpcsDto.nakano);
    this.syncNpcIfPresent(previousNpcs.seiryuLeader, newNpcsDto.seiryuLeader);
    this.syncNpcIfPresent(previousNpcs.keith, newNpcsDto.keith);
    this.syncNpcIfPresent(previousNpcs.suzakuLeader, newNpcsDto.suzakuLeader);
    this.syncNpcIfPresent(previousNpcs.fakeByakkoLeader, newNpcsDto.fakeByakkoLeader);
    this.syncNpcIfPresent(previousNpcs.byakkoLeader, newNpcsDto.byakkoLeader);
    this.syncNpcIfPresent(previousNpcs.aoaAttacker, newNpcsDto.aoaAttacker);
  }

  private static syncNpcIfPresent(previousNpc: Npc, newNpcDto: NpcDTO | undefined): void {
    if (newNpcDto === undefined) {
      return;
    }

    this.syncNpc(previousNpc, newNpcDto);
  }

  private static syncNpc(previousNpc: Npc, newNpcDto: NpcDTO): void {
    if (newNpcDto.battles === undefined) {
      return;
    }

    newNpcDto.battles.forEach((newBattleDto) => {
      const previousBattle = previousNpc.battles.find((battle) => {
        return battle.id === newBattleDto.id;
      });

      if (previousBattle) {
        this.syncBattle(previousBattle, newBattleDto);
      }
    });
  }

  private static syncBattle(previousBattle: NpcBattle, newBattleDto: NpcBattleDTO): void {
    if (newBattleDto.value !== undefined) {
      previousBattle.completed = newBattleDto.value !== 0;
    }
  }
}
