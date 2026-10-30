export const MapPreviews = [
  "location",
  "seabed",
  "seabedLongestNames",
  "mobiusDesert",
  "cardBattle",
  "digimonBattle",
] as const;

export type MapPreview = (typeof MapPreviews)[number];
