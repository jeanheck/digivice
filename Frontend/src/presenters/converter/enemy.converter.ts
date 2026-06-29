import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyConverter {
    public static convert(enemyRaw: EnemyRaw): EnemyViewModel {
        return {
            name: enemyRaw.name,
            level: enemyRaw.level,
            hp: enemyRaw.hp,
            mp: enemyRaw.mp,
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
                }
            },
            strDown: enemyRaw.strDown,
            defDown: enemyRaw.defDown,
            spdDown: enemyRaw.spdDown,
            dvxp: enemyRaw.dvxp,
            exp: enemyRaw.exp,
            bits: enemyRaw.bits,
            dropId: enemyRaw.dropId,
            regularAttackId: enemyRaw.regularAttackId,
            techniqueId: enemyRaw.techniqueId,
            boss: enemyRaw.boss,
        };
    }
}
