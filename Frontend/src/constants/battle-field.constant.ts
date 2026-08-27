import { ElementConstant } from "@/constants/stat/element.constant";

const battleFieldElementById: Readonly<Record<number, ElementConstant>> = {
  2: ElementConstant.fire,
  3: ElementConstant.water,
  4: ElementConstant.ice,
  5: ElementConstant.wind,
  6: ElementConstant.thunder,
  7: ElementConstant.machine,
  8: ElementConstant.dark,
};

const fieldTechniqueKeyByElement: Readonly<Record<ElementConstant, string>> = {
  [ElementConstant.fire]: "fireField",
  [ElementConstant.water]: "waterField",
  [ElementConstant.ice]: "iceField",
  [ElementConstant.wind]: "windField",
  [ElementConstant.thunder]: "thunderField",
  [ElementConstant.machine]: "metalField",
  [ElementConstant.dark]: "darkField",
};

export function resolveBattleFieldElement(fieldId: number): ElementConstant | null {
  return battleFieldElementById[fieldId] ?? null;
}

export function resolveFieldTechniqueKey(element: ElementConstant): string {
  return fieldTechniqueKeyByElement[element];
}

export function resolveBattleFieldAssetName(fieldId: number): string | null {
  if (fieldId === 0) {
    return "Neutral";
  }

  const element = resolveBattleFieldElement(fieldId);
  if (element === null) {
    return null;
  }

  return element.charAt(0).toUpperCase() + element.slice(1);
}
