export const DigimonElements = ["fire", "water", "ice", "wind", "thunder", "machine", "dark"] as const;

export type DigimonElement = (typeof DigimonElements)[number];
