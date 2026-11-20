import type { Npc, Npcs } from "@/state/models";
import type { NpcResource, NpcsResource } from "@/memory/resources/npcs.resource";

export class NpcsAssembler {
  private static assembleNpc(resource: NpcResource): Npc {
    return {
      battles: resource.battles.map((battle) => ({
        id: battle.id,
        won: battle.value !== 0,
      })),
    };
  }

  public static assemble(resource: NpcsResource): Npcs {
    const npcs = Object.entries(resource).map(([name, npcResource]) => [
      name,
      NpcsAssembler.assembleNpc(npcResource),
    ]);

    return Object.fromEntries(npcs) as Npcs;
  }
}
