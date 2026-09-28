export type CardType = "action" | "white" | "blue" | "green" | "red" | "black" | "brown";

export interface CardPointsRaw {
  ap: number;
  hp: number;
}

export interface CardRaw {
  imageName: string;
  type: CardType;
  points?: CardPointsRaw;
}
