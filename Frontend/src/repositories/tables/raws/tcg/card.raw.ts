export type CardType = "action" | "white" | "blue" | "green" | "red" | "black" | "brown";

export interface CardPointsRaw {
  ap: number;
  hp: number;
}

export interface CardShopRaw {
  id: string;
  startWhenLastMainQuestStepDone: string;
  finishWhenLastMainQuestStepDone: string;
}

export interface CardRaw {
  imageName: string;
  boosters: number[];
  cardShops?: CardShopRaw[];
  type: CardType;
  points?: CardPointsRaw;
}
