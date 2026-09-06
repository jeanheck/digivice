import { ImageCatalog } from "@/catalogs/image.catalog";
import { EnemySourceConstant } from "@/constants/enemy-source.constant";
import { IconConstant } from "@/constants/icon.constant";
import { toSpeciesConstant } from "@/constants/species.constant";
import type { Constant } from "@/constants/constant";
import type { Vital } from "@/models/party/digimon/vital";
import { EnemyConditionConverter } from "@/presenters/converter/enemy-condition.converter";
import { EnemyStatConverter } from "@/presenters/converter/enemy-stat.converter";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";
import type { DigimonBattleViewModel } from "@/viewmodels/map/digimon-battle.viewmodel";

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
    battleDeltas?: BattleEnemyDeltas,
  ): DigimonBattleViewModel {
    if (enemyRaw === null) {
      return {
        enemyId: null,
        title,
        isBoss: false,
        level: null,
        species: null,
        speciesEmoji: null,
        hp,
        attributes: [],
        elements: [],
        conditions: [],
        enemyImageUrl: null,
      };
    }

    const isBoss = enemyRaw.boss === true;
    const conditions = DigimonBattleConverter.toConditions(enemyRaw);

    return {
      enemyId,
      title,
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

  private static toSpeciesEmoji(species: string | null, isBoss: boolean): string | null {
    if (isBoss) {
      return IconConstant[EnemySourceConstant.boss];
    }

    const speciesConstant = toSpeciesConstant(species);
    if (speciesConstant === null) {
      return null;
    }

    return IconConstant[speciesConstant as Constant];
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
