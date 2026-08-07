import type { Vital } from "../party/digimon/vital";

export interface Enemy {
  id: number;
  condition: number;
  strength: number;
  defense: number;
  speed: number;
  hp: Vital;
}
