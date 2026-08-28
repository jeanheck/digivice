import NpcJson from "@/database/npc/npc.json";
import type { NpcTable } from "@/repositories/tables/npc/npc.table";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";

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
}
