import type { NpcBattleDTO } from "@/events/dto/npcs/npc-battle.dto";
import type { NpcDTO } from "@/events/dto/npcs/npc.dto";
import type { NpcsDTO } from "@/events/dto/npcs.dto";
import type { Npc, NpcBattle, Npcs } from "@/models";

export class NpcsConverter {
  public static convert(npcsDto: Required<NpcsDTO>): Npcs {
    return {
      genji: this.convertNpc(npcsDto.genji),
      natsumi: this.convertNpc(npcsDto.natsumi),
      catherine: this.convertNpc(npcsDto.catherine),
      lucia: this.convertNpc(npcsDto.lucia),
      robert: this.convertNpc(npcsDto.robert),
      akiba: this.convertNpc(npcsDto.akiba),
      chris: this.convertNpc(npcsDto.chris),
      tomomi: this.convertNpc(npcsDto.tomomi),
      mitch: this.convertNpc(npcsDto.mitch),
      bob: this.convertNpc(npcsDto.bob),
      andy: this.convertNpc(npcsDto.andy),
      george: this.convertNpc(npcsDto.george),
      meiLin: this.convertNpc(npcsDto.meiLin),
      jessica: this.convertNpc(npcsDto.jessica),
      gordon: this.convertNpc(npcsDto.gordon),
      alice: this.convertNpc(npcsDto.alice),
      nakano: this.convertNpc(npcsDto.nakano),
      seiryuLeader: this.convertNpc(npcsDto.seiryuLeader),
      keith: this.convertNpc(npcsDto.keith),
      suzakuLeader: this.convertNpc(npcsDto.suzakuLeader),
      fakeByakkoLeader: this.convertNpc(npcsDto.fakeByakkoLeader),
      byakkoLeader: this.convertNpc(npcsDto.byakkoLeader),
      aoaAttacker: this.convertNpc(npcsDto.aoaAttacker),
    };
  }

  public static convertNpc(npcDto: NpcDTO | undefined): Npc {
    return {
      battles: (npcDto?.battles ?? []).map((battleDto) => {
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
