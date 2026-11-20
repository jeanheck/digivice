import type { DigievolutionSlot, Digimon, Equipments, StoredDigievolution } from "@/state/models";
import type { DigimonResource } from "@/memory/resources/parties/digimon.resource";
import type { DigievolutionSlotResource } from "@/memory/resources/parties/digimons/digievolution-slot.resource";
import type { EquipmentsResource } from "@/memory/resources/parties/digimons/equipments.resource";
import type { StoredDigievolutionResource } from "@/memory/resources/parties/digimons/stored-digievolution.resource";

export class DigimonAssembler {
  private static toNullWhenNotPositive(value: number): number | null {
    return value <= 0 ? null : value;
  }

  private static assembleEquipments(resource: EquipmentsResource): Equipments {
    return {
      head: DigimonAssembler.toNullWhenNotPositive(resource.head),
      body: DigimonAssembler.toNullWhenNotPositive(resource.body),
      right: DigimonAssembler.toNullWhenNotPositive(resource.right),
      left: DigimonAssembler.toNullWhenNotPositive(resource.left),
      accessory1: DigimonAssembler.toNullWhenNotPositive(resource.accessory1),
      accessory2: DigimonAssembler.toNullWhenNotPositive(resource.accessory2),
    };
  }

  private static assembleDigievolutionSlot(
    resource: DigievolutionSlotResource,
    storedDigievolutions: StoredDigievolutionResource[],
  ): DigievolutionSlot {
    if (resource.digievolutionId <= 0) {
      return { index: resource.index, digievolutionId: null, digievolution: null };
    }

    const storedDigievolution = storedDigievolutions.find(
      (stored) => stored.digievolutionId === resource.digievolutionId,
    );

    return {
      index: resource.index,
      digievolutionId: resource.digievolutionId,
      digievolution: {
        level: storedDigievolution?.level ?? 1,
        dvexp: Math.max(0, storedDigievolution?.dvexp ?? 0),
      },
    };
  }

  private static assembleStoredDigievolution(
    resource: StoredDigievolutionResource,
  ): StoredDigievolution {
    return {
      digievolutionId: resource.digievolutionId,
      level: resource.level,
    };
  }

  public static assemble(resource: DigimonResource): Digimon {
    return {
      level: resource.level,
      tp: resource.tp,
      blast: resource.blast,
      experience: resource.experience,
      hp: { current: resource.hp.current, max: resource.hp.max },
      mp: { current: resource.mp.current, max: resource.mp.max },
      inBattle: {
        condition: resource.inBattle.condition,
        strength: resource.inBattle.strength,
        defense: resource.inBattle.defense,
        speed: resource.inBattle.speed,
        hp: { current: resource.inBattle.hp.current, max: resource.inBattle.hp.max },
        mp: { current: resource.inBattle.mp.current, max: resource.inBattle.mp.max },
      },
      attributes: { ...resource.attributes },
      elements: { ...resource.elements },
      equipments: DigimonAssembler.assembleEquipments(resource.equipments),
      digievolutions: resource.digievolutions.map((slot) =>
        DigimonAssembler.assembleDigievolutionSlot(slot, resource.storedDigievolutions),
      ),
      storedDigievolutions: resource.storedDigievolutions.map((stored) =>
        DigimonAssembler.assembleStoredDigievolution(stored),
      ),
      activeDigievolutionId: DigimonAssembler.toNullWhenNotPositive(resource.activeDigievolutionId),
    };
  }
}
