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

export function resolveBattleFieldElement(fieldId: number): ElementConstant | null {
  return battleFieldElementById[fieldId] ?? null;
}

export function resolveBattleFieldAssetName(fieldId: number): string | null {
  const element = resolveBattleFieldElement(fieldId);
  if (element === null) {
    return null;
  }

  return element.charAt(0).toUpperCase() + element.slice(1);
}
