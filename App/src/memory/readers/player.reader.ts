import type { PlayerAddresses } from "@/memory/addresses/player.addresses";
import type { PlayerResource } from "@/memory/resources/player.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class PlayerReader {
  public static read(snapshot: MemorySnapshot, addresses: PlayerAddresses): PlayerResource {
    return {
      bits: snapshot.readInt32(addresses.bits),
      mapId: snapshot.readInt16(addresses.mapId),
      previousMapId: snapshot.readInt16(addresses.previousMapId),
      seabedRoute: snapshot.readByte(addresses.seabedRoute),
      mapVariant: snapshot.readByte(addresses.mapVariant),
    };
  }
}
