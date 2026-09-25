import type { Vital } from "./vital";
import type { InBattle } from "./in-battle";
import type { Attributes } from "./attributes";
import type { Resistances } from "./resistances";
import type { Equipments } from "./equipments";
import type { DigievolutionSlot } from "./digievolution-slot";
import type { StoredDigievolution } from "./stored-digievolution";

export interface Digimon {
  level: number;
  tp: number;
  blast: number;
  experience: number;
  hp: Vital;
  mp: Vital;
  inBattle: InBattle;
  attributes: Attributes;
  resistances: Resistances;
  equipments: Equipments;
  digievolutions: DigievolutionSlot[];
  storedDigievolutions: StoredDigievolution[];
  activeDigievolutionId: number | null;
}
