import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { DigimonSpeciesIcon, EnemySourceIcon } from "@/shared/constants/icon.constant";
import type { DigimonElement } from "@/shared/constants/digimon-element.constant";
import type { Vital } from "@/state/models";
import { EnemyConditionConverter } from "@/presentation/presenters/converter/enemy-condition.converter";
import { EnemyStatConverter } from "@/presentation/presenters/converter/enemy-stat.converter";
import type { EnemyRaw } from "@/presentation/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyAffinities } from "@/presentation/services/enemy-affinity.service";
import type { DigimonSpecies } from "@/shared/types/digimon-species.type";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/presentation/viewmodels/enemy/enemy-stat.viewmodel";
import type { DigimonBattleViewModel } from "@/presentation/viewmodels/map/digimon-battle.viewmodel";
import type { EnemyAffinityViewModel } from "@/presentation/viewmodels/enemy/enemy-affinities.viewmodel";

export interface BattleEnemyDeltas {
  strength: number;
  defense: number;
  speed: number;
}

export class DigimonBattleConverter {
  public static convert(
    enemyRaw: EnemyRaw | null,
    hp: Vital,
    title: string,
    enemyId: string | null,
    affinities: EnemyAffinities,
    battleDeltas?: BattleEnemyDeltas,
  ): DigimonBattleViewModel {
    if (enemyRaw === null) {
      return {
        enemyId: null,
        enemyName: title,
        isBoss: false,
        level: null,
        species: null,
        speciesEmoji: null,
        hp,
        attributes: [],
        elements: [],
        conditions: [],
        affinities: { effective: [], notEffective: [] },
        enemyImageUrl: null,
      };
    }

    const isBoss = enemyRaw.boss === true;
    const conditions = DigimonBattleConverter.toConditions(enemyRaw);

    return {
      enemyId,
      enemyName: title,
      isBoss,
      level: enemyRaw.level,
      species: enemyRaw.species,
      speciesEmoji: DigimonBattleConverter.toSpeciesEmoji(enemyRaw.species, isBoss),
      hp,
      attributes: DigimonBattleConverter.applyBattleDeltas(
        EnemyStatConverter.convertAttributes({
          strength: enemyRaw.strength,
          defense: enemyRaw.defense,
          spirit: enemyRaw.spirit,
          wisdom: enemyRaw.wisdom,
          speed: enemyRaw.speed,
        }),
        battleDeltas,
      ),
      elements: EnemyStatConverter.convertElements({
        fire: enemyRaw.fire,
        water: enemyRaw.water,
        ice: enemyRaw.ice,
        wind: enemyRaw.wind,
        thunder: enemyRaw.thunder,
        machine: enemyRaw.machine,
        dark: enemyRaw.dark,
      }),
      conditions: EnemyConditionConverter.convertConditions(conditions),
      affinities: {
        effective: DigimonBattleConverter.toAffinities(affinities.effective),
        notEffective: DigimonBattleConverter.toAffinities(affinities.notEffective),
      },
      enemyImageUrl: ImageCatalog.getEnemyImageUrl(enemyRaw.name),
    };
  }

  private static applyBattleDeltas(
    attributes: EnemyStatViewModel[],
    battleDeltas?: BattleEnemyDeltas,
  ): EnemyStatViewModel[] {
    if (battleDeltas === undefined) {
      return attributes;
    }

    const deltaByStatKey: Record<string, number> = {
      strength: battleDeltas.strength,
      defense: battleDeltas.defense,
      speed: battleDeltas.speed,
    };

    return attributes.map((stat) => {
      const delta = deltaByStatKey[stat.statKey];
      if (delta === undefined) {
        return stat;
      }

      const baseValue = stat.value;

      return {
        ...stat,
        baseValue,
        delta,
        value: baseValue + delta,
      };
    });
  }

  private static toAffinities(elements: DigimonElement[]): EnemyAffinityViewModel[] {
    return elements.map((element) => {
      return {
        element,
        iconUrl: ImageCatalog.getStatImageUrl(element),
      };
    });
  }

  private static toSpeciesEmoji(species: DigimonSpecies, isBoss: boolean): string {
    if (isBoss) {
      return EnemySourceIcon.boss;
    }

    return DigimonSpeciesIcon[species];
  }

  private static toConditions(enemyRaw: EnemyRaw): EnemyViewModel["conditions"] {
    return {
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
    };
  }
}
