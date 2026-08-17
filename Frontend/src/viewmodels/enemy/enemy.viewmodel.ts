import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";

export interface EnemyViewModel {
  name: string;
  level: number;
  hp: number;
  species: string;
  rate: number;
  attributes: {
    strength: number;
    defense: number;
    spirit: number;
    wisdom: number;
    speed: number;
  };
  elements: {
    fire: number;
    water: number;
    ice: number;
    wind: number;
    thunder: number;
    machine: number;
    dark: number;
  };
  conditions: {
    poison: {
      can: boolean;
      value: number;
    };
    paralyze: {
      can: boolean;
      value: number;
    };
    confuse: {
      can: boolean;
      value: number;
    };
    sleep: {
      can: boolean;
      value: number;
    };
    ko: {
      can: boolean;
      value: number;
    };
    drain: {
      can: boolean;
    };
    steal: {
      can: boolean;
    };
    escape: {
      can: boolean;
    };
  };
  strDown: string;
  defDown: string;
  spdDown: string;
  dvxp: number;
  exp: number;
  bits: number;
  drops?: EnemyDropViewModel[];
  locations?: EnemyLocationViewModel[];
  regularAttackId: string | null;
  techniqueId: string | null;
  boss: boolean;
}
