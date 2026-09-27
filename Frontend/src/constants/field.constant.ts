import type { DigimonElement } from "@/constants/digimon-element.constant";

const fieldElementById: Readonly<Record<number, DigimonElement>> = {
  2: "fire",
  3: "water",
  4: "ice",
  5: "wind",
  6: "thunder",
  7: "machine",
  8: "dark",
};

function resolveFieldElement(fieldId: number): DigimonElement | null {
  return fieldElementById[fieldId] ?? null;
}

export function resolveFieldAssetName(fieldId: number): string | null {
  if (fieldId === 0) {
    return "Neutral";
  }

  const element = resolveFieldElement(fieldId);
  if (element === null) {
    return null;
  }

  return element.charAt(0).toUpperCase() + element.slice(1);
}
