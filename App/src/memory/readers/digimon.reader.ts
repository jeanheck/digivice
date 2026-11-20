import type { DigimonAddress } from "@/memory/addresses/parties/digimon.addresses";
import type { DigimonStatusAddresses } from "@/memory/addresses/parties/digimon-status.addresses";
import type { DigievolutionsAddresses } from "@/memory/addresses/parties/digimons/digievolutions.addresses";
import type { InBattleAddresses } from "@/memory/addresses/parties/in-battle.addresses";
import { InBattleReader } from "@/memory/readers/in-battle.reader";
import type { DigimonResource } from "@/memory/resources/parties/digimon.resource";
import type { DigievolutionSlotResource } from "@/memory/resources/parties/digimons/digievolution-slot.resource";
import type { StoredDigievolutionResource } from "@/memory/resources/parties/digimons/stored-digievolution.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class DigimonReader {
  private static readDigievolutionSlots(
    snapshot: MemorySnapshot,
    memoryBlockAddress: number,
    addresses: DigievolutionsAddresses,
  ): DigievolutionSlotResource[] {
    return addresses.slots.map((slot) => ({
      index: slot.index,
      digievolutionId: snapshot.readInt16(memoryBlockAddress + slot.address),
    }));
  }

  private static readStoredDigievolutions(
    snapshot: MemorySnapshot,
    memoryBlockAddress: number,
    addresses: DigievolutionsAddresses,
  ): StoredDigievolutionResource[] {
    const storedDigievolutions: StoredDigievolutionResource[] = [];

    for (let entryIndex = 0; entryIndex < addresses.maxUnlockedDigievolutions; entryIndex++) {
      const entryAddress =
        memoryBlockAddress +
        addresses.unlockedDigievolutionsStart +
        entryIndex * addresses.unlockedDigievolutionEntryStride;
      const digievolutionId = snapshot.readInt16(entryAddress + addresses.id);

      if (digievolutionId <= 0) {
        break;
      }

      storedDigievolutions.push({
        digievolutionId,
        level: snapshot.readInt16(entryAddress + addresses.level),
        dvexp: snapshot.readInt32(entryAddress + addresses.dvexp),
      });
    }

    return storedDigievolutions;
  }

  public static read(
    snapshot: MemorySnapshot,
    digimonAddress: DigimonAddress,
    statusAddresses: DigimonStatusAddresses,
    inBattleAddresses: InBattleAddresses,
    partySlotIndex: number,
  ): DigimonResource {
    const blockAddress = digimonAddress.memoryBlockAddress;
    const readInt16 = (offset: number): number => snapshot.readInt16(blockAddress + offset);

    return {
      activeDigievolutionId: readInt16(statusAddresses.digievolutions.activeDigievolution),
      experience: snapshot.readInt32(blockAddress + statusAddresses.experience),
      level: readInt16(statusAddresses.level),
      tp: readInt16(statusAddresses.tp),
      blast: snapshot.readInt16(digimonAddress.blastAddress),
      hp: {
        current: readInt16(statusAddresses.hp.current),
        max: readInt16(statusAddresses.hp.max),
      },
      mp: {
        current: readInt16(statusAddresses.mp.current),
        max: readInt16(statusAddresses.mp.max),
      },
      inBattle: InBattleReader.read(snapshot, inBattleAddresses, partySlotIndex),
      attributes: {
        strength: readInt16(statusAddresses.attributes.strength),
        defense: readInt16(statusAddresses.attributes.defense),
        spirit: readInt16(statusAddresses.attributes.spirit),
        wisdom: readInt16(statusAddresses.attributes.wisdom),
        speed: readInt16(statusAddresses.attributes.speed),
        charisma: readInt16(statusAddresses.attributes.charisma),
      },
      elements: {
        fire: readInt16(statusAddresses.elements.fire),
        water: readInt16(statusAddresses.elements.water),
        ice: readInt16(statusAddresses.elements.ice),
        wind: readInt16(statusAddresses.elements.wind),
        thunder: readInt16(statusAddresses.elements.thunder),
        machine: readInt16(statusAddresses.elements.machine),
        dark: readInt16(statusAddresses.elements.dark),
      },
      equipments: {
        head: readInt16(statusAddresses.equipments.head),
        body: readInt16(statusAddresses.equipments.body),
        right: readInt16(statusAddresses.equipments.right),
        left: readInt16(statusAddresses.equipments.left),
        accessory1: readInt16(statusAddresses.equipments.accessory1),
        accessory2: readInt16(statusAddresses.equipments.accessory2),
      },
      digievolutions: DigimonReader.readDigievolutionSlots(
        snapshot,
        blockAddress,
        statusAddresses.digievolutions,
      ),
      storedDigievolutions: DigimonReader.readStoredDigievolutions(
        snapshot,
        blockAddress,
        statusAddresses.digievolutions,
      ),
    };
  }
}
