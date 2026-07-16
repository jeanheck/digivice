export type DesertAreaType = "noiseDesert" | "mirageTower" | "normal" | "border";

export interface DesertAreaDefinition {
  label: string;
  type: DesertAreaType;
}

export const DESERT_GRID_SIZE = 6;

export const DESERT_AREAS: DesertAreaDefinition[][] = [
  [
    { label: "D4", type: "border" },
    { label: "D1", type: "border" },
    { label: "D2", type: "border" },
    { label: "noiseDesert", type: "noiseDesert" },
    { label: "D4", type: "border" },
    { label: "D1", type: "border" },
  ],
  [
    { label: "A4", type: "border" },
    { label: "A1", type: "normal" },
    { label: "A2", type: "normal" },
    { label: "A3", type: "normal" },
    { label: "A4", type: "normal" },
    { label: "A1", type: "border" },
  ],
  [
    { label: "B4", type: "border" },
    { label: "B1", type: "normal" },
    { label: "B2", type: "normal" },
    { label: "B3", type: "normal" },
    { label: "B4", type: "normal" },
    { label: "B1", type: "border" },
  ],
  [
    { label: "mirageTower", type: "mirageTower" },
    { label: "C1", type: "normal" },
    { label: "C2", type: "normal" },
    { label: "C3", type: "normal" },
    { label: "C4", type: "normal" },
    { label: "C1", type: "border" },
  ],
  [
    { label: "D4", type: "border" },
    { label: "D1", type: "normal" },
    { label: "D2", type: "normal" },
    { label: "D3", type: "normal" },
    { label: "D4", type: "normal" },
    { label: "D1", type: "border" },
  ],
  [
    { label: "A4", type: "border" },
    { label: "A1", type: "border" },
    { label: "A2", type: "border" },
    { label: "A3", type: "border" },
    { label: "A4", type: "border" },
    { label: "A1", type: "border" },
  ],
];
