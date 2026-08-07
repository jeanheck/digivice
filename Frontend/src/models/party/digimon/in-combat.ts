import type { Vital } from "./vital";

export interface InCombat {
  condition: number;
  strength: number;
  defense: number;
  speed: number;
  hp: Vital;
  mp: Vital;
}
