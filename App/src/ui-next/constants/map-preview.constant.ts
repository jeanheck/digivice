export const MapPreviews = [
  "location",
  "seabed",
  "mobiusDesert",
  "cardBattle",
  "digimonBattle",
] as const;

export type MapPreview = (typeof MapPreviews)[number];
