import NpcJson from "@/data/game/npc/npc.json";
import type { NpcTable } from "@/presentation/repositories/tables/npc/npc.table";
import type { NpcRaw } from "@/presentation/repositories/tables/raws/npc/npc.raw";

export class NpcRepository {
  private static readonly npcTable = NpcJson as NpcTable;

  public static getNpcById(npcId: string): NpcRaw | undefined {
    return this.npcTable[npcId];
  }

  public static getNpcTable(): NpcTable {
    return this.npcTable;
  }

  public static getNpcIds(): string[] {
    return Object.keys(this.npcTable);
  }

  public static getNpcIdsByLocationId(locationId: string): string[] {
    return Object.entries(this.npcTable)
      .filter(([, npcRaw]) => {
        return npcRaw.locationId === locationId;
      })
      .map(([npcId]) => {
        return npcId;
      });
  }
}
