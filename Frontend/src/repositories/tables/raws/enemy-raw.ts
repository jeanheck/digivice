export interface EnemyDropRaw {
    id: string;
}

export interface EnemyLocalizedTextRaw {
    "PT-BR": string;
    "EN-US": string;
}

export interface EnemyRaw {
    name: string;
    level: number;
    hp: number;
    mp: number;
    species: string;
    rate: number;
    strength: number;
    defense: number;
    spirit: number;
    wisdom: number;
    speed: number;
    fire: number;
    water: number;
    ice: number;
    wind: number;
    thunder: number;
    machine: number;
    dark: number;
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
    strDown: string;
    defDown: string;
    spdDown: string;
    canEscape: boolean;
    dvxp: number;
    exp: number;
    bits: number;
    location: string[];
    regularAttack?: EnemyLocalizedTextRaw;
    technique?: EnemyLocalizedTextRaw;
    boss?: boolean;
    drops: EnemyDropRaw | null;
}
