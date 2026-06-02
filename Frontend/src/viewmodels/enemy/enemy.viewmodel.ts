export interface EnemyViewModel {
    name: string;
    level: number;
    hp: number;
    mp: number;
    species: string;
    rate: number;
    attributes: {
        strength: number;
        defense: number;
        spirit: number;
        wisdom: number;
        speed: number;
    },
    elements: {
        fire: number;
        water: number;
        ice: number;
        wind: number;
        thunder: number;
        machine: number;
        dark: number;
    },
    conditions: {
        canPoison: boolean;
        poison: number;
        canParalyze: boolean;
        paralyze: number;
        canConfuse: boolean;
        confuse: number;
        canSleep: boolean;
        sleep: number;
        canKO: boolean;
        ko: number;
        canDrain: boolean;
        canSteal: boolean;
        canEscape: boolean;
    }
    strDown: string;
    defDown: string;
    spdDown: string;
    dvxp: number;
    exp: number;
    bits: number;
    dropId: string | null;
    regularAttackId: string | null;
    techniqueId: string | null;
    boss: boolean;
}
