export interface EnemyDropViewModel {
    id: string;
}

export interface EnemyLocalizedTextViewModel {
    "PT-BR": string;
    "EN-US": string;
}

export interface EnemyViewModel {
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
    regularAttack?: EnemyLocalizedTextViewModel;
    technique?: EnemyLocalizedTextViewModel;
    boss?: boolean;
    drops: EnemyDropViewModel | null;
}
