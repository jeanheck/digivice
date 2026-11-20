import type { AttributesResource } from "@/memory/resources/parties/digimons/attributes.resource";
import type { DigievolutionSlotResource } from "@/memory/resources/parties/digimons/digievolution-slot.resource";
import type { ElementsResource } from "@/memory/resources/parties/digimons/elements.resource";
import type { EquipmentsResource } from "@/memory/resources/parties/digimons/equipments.resource";
import type { InBattleResource } from "@/memory/resources/parties/digimons/in-battle.resource";
import type { StoredDigievolutionResource } from "@/memory/resources/parties/digimons/stored-digievolution.resource";
import type { VitalResource } from "@/memory/resources/parties/digimons/vital.resource";

export interface DigimonResource {
  activeDigievolutionId: number;
  experience: number;
  level: number;
  tp: number;
  blast: number;
  hp: VitalResource;
  mp: VitalResource;
  inBattle: InBattleResource;
  attributes: AttributesResource;
  elements: ElementsResource;
  equipments: EquipmentsResource;
  digievolutions: DigievolutionSlotResource[];
  storedDigievolutions: StoredDigievolutionResource[];
}
