import NpcsJson from "@/database/npc/npcs.json";
import type { NpcsTable } from "@/repositories/tables/npc/npcs.table";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";

export class NpcRepository {
  private static readonly npcsTable = NpcsJson as NpcsTable;

  public static getNpcById(npcId: string): NpcRaw | undefined {
    return this.npcsTable[npcId];
  }

  public static getNpcTable(): NpcsTable {
    return this.npcsTable;
  }

  public static getNpcIds(): string[] {
    return Object.keys(this.npcsTable);
  }
}
