export type CardType = "action" | "white" | "blue" | "green" | "red" | "black" | "brown";

export interface CardPointsRaw {
  ap: number;
  hp: number;
}

export interface CardRaw {
  boosters: number[];
  stores?: string[];
  type: CardType;
  points?: CardPointsRaw;
}
