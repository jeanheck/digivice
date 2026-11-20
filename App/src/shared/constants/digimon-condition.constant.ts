export const DigimonConditions = [
  "poison",
  "paralyze",
  "confuse",
  "sleep",
  "ko",
  "drain",
  "steal",
  "escape",
] as const;

export type DigimonCondition = (typeof DigimonConditions)[number];
