import type { NpcAddresses, NpcsAddresses } from "@/memory/addresses/npcs.addresses";
import type { NpcResource, NpcsResource } from "@/memory/resources/npcs.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class NpcsReader {
  private static readNpc(snapshot: MemorySnapshot, addresses: NpcAddresses): NpcResource {
    // Battle ids are non-numeric keys, so Object.entries keeps the JSON order.
    return {
      battles: Object.entries(addresses.battles).map(([id, battle]) => ({
        id,
        value: snapshot.readByte(battle.address, battle.bitMask),
      })),
    };
  }

  public static read(snapshot: MemorySnapshot, addresses: NpcsAddresses): NpcsResource {
    const npcs = Object.entries(addresses).map(([name, npcAddresses]) => [
      name,
      NpcsReader.readNpc(snapshot, npcAddresses),
    ]);

    return Object.fromEntries(npcs) as NpcsResource;
  }
}
