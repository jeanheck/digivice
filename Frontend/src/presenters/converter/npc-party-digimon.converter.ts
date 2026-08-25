import type { NpcPartyDigimonRaw } from "@/repositories/tables/raws/npc/npc-party-digimon.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class NpcPartyDigimonConverter {
  public static convert(partyDigimonRaw: NpcPartyDigimonRaw): EnemyViewModel {
    return {
      name: partyDigimonRaw.name,
      level: partyDigimonRaw.level,
      hp: partyDigimonRaw.hp,
      species: partyDigimonRaw.species,
      rate: partyDigimonRaw.rate,
      attributes: {
        strength: partyDigimonRaw.strength,
        defense: partyDigimonRaw.defense,
        spirit: partyDigimonRaw.spirit,
        wisdom: partyDigimonRaw.wisdom,
        speed: partyDigimonRaw.speed,
      },
      elements: {
        fire: partyDigimonRaw.fire,
        water: partyDigimonRaw.water,
        ice: partyDigimonRaw.ice,
        wind: partyDigimonRaw.wind,
        thunder: partyDigimonRaw.thunder,
        machine: partyDigimonRaw.machine,
        dark: partyDigimonRaw.dark,
      },
      conditions: {
        poison: {
          can: partyDigimonRaw.canPoison,
          value: partyDigimonRaw.poison,
        },
        paralyze: {
          can: partyDigimonRaw.canParalyze,
          value: partyDigimonRaw.paralyze,
        },
        confuse: {
          can: partyDigimonRaw.canConfuse,
          value: partyDigimonRaw.confuse,
        },
        sleep: {
          can: partyDigimonRaw.canSleep,
          value: partyDigimonRaw.sleep,
        },
        ko: {
          can: partyDigimonRaw.canKO,
          value: partyDigimonRaw.ko,
        },
        drain: {
          can: partyDigimonRaw.canDrain,
        },
        steal: {
          can: partyDigimonRaw.canSteal,
        },
        escape: {
          can: partyDigimonRaw.canEscape,
        },
      },
      strDown: partyDigimonRaw.strDown,
      defDown: partyDigimonRaw.defDown,
      spdDown: partyDigimonRaw.spdDown,
      dvxp: 0,
      exp: 0,
      bits: 0,
      drops: partyDigimonRaw.drops?.map((dropRaw) => {
        return {
          id: dropRaw.id,
          locationOnly: dropRaw.locationOnly,
        };
      }),
      locations: [],
      regularAttackId: partyDigimonRaw.regularAttackId,
      techniqueId: partyDigimonRaw.techniqueId,
      boss: false,
    };
  }
}
