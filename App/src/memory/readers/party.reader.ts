import type { DigimonsAddresses } from "@/memory/addresses/parties/digimon.addresses";
import type { DigimonStatusAddresses } from "@/memory/addresses/parties/digimon-status.addresses";
import type { InBattleAddresses } from "@/memory/addresses/parties/in-battle.addresses";
import type { SlotAddresses } from "@/memory/addresses/parties/slot.addresses";
import type { PartyAddresses } from "@/memory/addresses/party.addresses";
import { DigimonReader } from "@/memory/readers/digimon.reader";
import type { PartyResource } from "@/memory/resources/party.resource";
import type { DigimonSlotResource } from "@/memory/resources/parties/digimon-slot.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class PartyReader {
  private static readSlot(
    snapshot: MemorySnapshot,
    slot: SlotAddresses,
    partyAddresses: PartyAddresses,
    digimonsAddresses: DigimonsAddresses,
    digimonStatusAddresses: DigimonStatusAddresses,
    inBattleAddresses: InBattleAddresses,
  ): DigimonSlotResource {
    const digimonId = snapshot.readByte(slot.address);
    const digimonAddress =
      digimonId === partyAddresses.emptySlotId ? undefined : digimonsAddresses.get(digimonId);

    return {
      index: slot.index,
      digimonId,
      digimon:
        digimonAddress === undefined
          ? null
          : DigimonReader.read(
              snapshot,
              digimonAddress,
              digimonStatusAddresses,
              inBattleAddresses,
              slot.index,
            ),
    };
  }

  public static read(
    snapshot: MemorySnapshot,
    partyAddresses: PartyAddresses,
    digimonsAddresses: DigimonsAddresses,
    digimonStatusAddresses: DigimonStatusAddresses,
    inBattleAddresses: InBattleAddresses,
  ): PartyResource {
    return {
      slots: partyAddresses.slots.map((slot) =>
        PartyReader.readSlot(
          snapshot,
          slot,
          partyAddresses,
          digimonsAddresses,
          digimonStatusAddresses,
          inBattleAddresses,
        ),
      ),
    };
  }
}
