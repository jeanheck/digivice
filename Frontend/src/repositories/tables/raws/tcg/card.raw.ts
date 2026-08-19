export type CardType = "action" | "white" | "blue" | "green" | "red" | "black" | "brown";

export interface CardPointsRaw {
  ap: number;
  hp: number;
}

export interface CardRaw {
  boosters: number[];
  type: CardType;
  points?: CardPointsRaw;
}
