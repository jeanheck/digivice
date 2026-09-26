export const DigimonAttributes = ["strength", "defense", "spirit", "wisdom", "speed", "charisma"] as const;

export type DigimonAttribute = (typeof DigimonAttributes)[number];

export const EnemyAttributes = [
  "strength",
  "defense",
  "spirit",
  "wisdom",
  "speed",
] as const satisfies readonly DigimonAttribute[];

export type EnemyAttribute = (typeof EnemyAttributes)[number];
