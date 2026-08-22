import NpcPartyJson from "@/database/npc/npc-party.json";
import type { NpcPartyTable } from "@/repositories/tables/npc/npc-party.table";
import type { NpcPartyRaw } from "@/repositories/tables/raws/npc/npc-party.raw";

export class NpcPartyRepository {
  private static readonly npcPartyTable = NpcPartyJson as NpcPartyTable;

  public static getPartyById(partyId: string): NpcPartyRaw | undefined {
    return this.npcPartyTable[partyId];
  }

  public static getNpcPartyTable(): NpcPartyTable {
    return this.npcPartyTable;
  }
}
