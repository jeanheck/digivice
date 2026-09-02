import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyConverter {
  public static convert(enemyRaw: EnemyRaw): EnemyViewModel {
    const enemyViewModel: EnemyViewModel = {
      name: enemyRaw.name,
      level: enemyRaw.level,
      hp: enemyRaw.hp,
      species: enemyRaw.species,
      rate: enemyRaw.rate,
      attributes: {
        strength: enemyRaw.strength,
        defense: enemyRaw.defense,
        spirit: enemyRaw.spirit,
        wisdom: enemyRaw.wisdom,
        speed: enemyRaw.speed,
      },
      elements: {
        fire: enemyRaw.fire,
        water: enemyRaw.water,
        ice: enemyRaw.ice,
        wind: enemyRaw.wind,
        thunder: enemyRaw.thunder,
        machine: enemyRaw.machine,
        dark: enemyRaw.dark,
      },
      conditions: {
        poison: {
          can: enemyRaw.canPoison,
          value: enemyRaw.poison,
        },
        paralyze: {
          can: enemyRaw.canParalyze,
          value: enemyRaw.paralyze,
        },
        confuse: {
          can: enemyRaw.canConfuse,
          value: enemyRaw.confuse,
        },
        sleep: {
          can: enemyRaw.canSleep,
          value: enemyRaw.sleep,
        },
        ko: {
          can: enemyRaw.canKO,
          value: enemyRaw.ko,
        },
        drain: {
          can: enemyRaw.canDrain,
        },
        steal: {
          can: enemyRaw.canSteal,
        },
        escape: {
          can: enemyRaw.canEscape,
        },
      },
      strDown: enemyRaw.strDown,
      defDown: enemyRaw.defDown,
      spdDown: enemyRaw.spdDown,
      regularAttackId: enemyRaw.regularAttackId,
      techniqueId: enemyRaw.techniqueId,
      boss: enemyRaw.boss === true,
    };

    if (enemyRaw.dvxp !== undefined) {
      enemyViewModel.dvxp = enemyRaw.dvxp;
    }
    if (enemyRaw.exp !== undefined) {
      enemyViewModel.exp = enemyRaw.exp;
    }
    if (enemyRaw.bits !== undefined) {
      enemyViewModel.bits = enemyRaw.bits;
    }
    if (enemyRaw.drops !== undefined) {
      enemyViewModel.drops = enemyRaw.drops.map((dropRaw) => {
        return {
          id: dropRaw.id,
          locationOnly: dropRaw.locationOnly,
        };
      });
    }
    if (enemyRaw.locations !== undefined) {
      enemyViewModel.locations = enemyRaw.locations.map((locationRaw) => {
        return {
          id: locationRaw.id,
          sources: locationRaw.sources,
          localCoordinates:
            locationRaw.localCoordinates != null
              ? {
                  x: locationRaw.localCoordinates.x,
                  y: locationRaw.localCoordinates.y,
                }
              : undefined,
          mainQuestStepDone: locationRaw.mainQuestStepDone,
        };
      });
    }
    if (enemyRaw.tamerId !== undefined) {
      enemyViewModel.tamerId = enemyRaw.tamerId;
    }
    if (enemyRaw.npcId !== undefined) {
      enemyViewModel.npcId = enemyRaw.npcId;
    }

    return enemyViewModel;
  }
}
