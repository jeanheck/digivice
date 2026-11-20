export const DigimonDebuff = {
  poison: 1,
  paralyze: 2,
  confuse: 4,
  sleep: 8,
} as const;

export type DigimonDebuff = keyof typeof DigimonDebuff;
