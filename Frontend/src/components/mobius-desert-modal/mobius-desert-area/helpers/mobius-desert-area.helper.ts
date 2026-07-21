import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";

export function getConnectionColorClasse(
  sourceType: DesertAreaTypeViewModel,
  targetType: DesertAreaTypeViewModel | null
): string {
  if (sourceType === "border" || targetType === "border") {
    return "bg-gray-500";
  }

  if (sourceType === "noiseDesertS" || targetType === "noiseDesertS") {
    return "bg-green-500";
  }

  if (sourceType === "mirageTower" || targetType === "mirageTower") {
    return "bg-cyan-400";
  }

  return "bg-cyan-500";
}
