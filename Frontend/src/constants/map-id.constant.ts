export const MapId = {
  digimonBattle: "0600",
  cardBattle: "0700",
  undergroundPath: "020B",
  asukaSewers: "021B",
} as const;

export type MapId = (typeof MapId)[keyof typeof MapId];
