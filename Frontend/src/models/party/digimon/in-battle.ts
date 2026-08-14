import type { Vital } from "./vital";

export interface InBattle {
  condition: number;
  strength: number;
  defense: number;
  speed: number;
  hp: Vital;
  mp: Vital;
}
