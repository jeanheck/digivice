import type { DigimonElement } from "@/constants/digimon-element.constant";

const battleFieldElementById: Readonly<Record<number, DigimonElement>> = {
  2: "fire",
  3: "water",
  4: "ice",
  5: "wind",
  6: "thunder",
  7: "machine",
  8: "dark",
};

function resolveBattleFieldElement(fieldId: number): DigimonElement | null {
  return battleFieldElementById[fieldId] ?? null;
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
