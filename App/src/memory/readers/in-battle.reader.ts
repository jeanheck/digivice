import type { InBattleAddresses } from "@/memory/addresses/parties/in-battle.addresses";
import type { InBattleResource } from "@/memory/resources/parties/digimons/in-battle.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class InBattleReader {
  public static read(
    snapshot: MemorySnapshot,
    addresses: InBattleAddresses,
    partySlotIndex: number,
  ): InBattleResource {
    // Party slot indexes start at 1.
    const slotBase = addresses.allySlotBase + (partySlotIndex - 1) * addresses.slotStride;

    return {
      condition: snapshot.readByte(slotBase + addresses.condition),
      strength: snapshot.readInt16(slotBase + addresses.strength),
      defense: snapshot.readInt16(slotBase + addresses.defense),
      speed: snapshot.readInt16(slotBase + addresses.speed),
      hp: {
        max: snapshot.readInt16(slotBase + addresses.hp.max),
        current: snapshot.readInt16(slotBase + addresses.hp.current),
      },
      mp: {
        max: snapshot.readInt16(slotBase + addresses.mp.max),
        current: snapshot.readInt16(slotBase + addresses.mp.current),
      },
    };
  }
}
