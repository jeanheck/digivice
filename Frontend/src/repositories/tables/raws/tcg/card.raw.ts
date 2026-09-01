export type CardType = "action" | "white" | "blue" | "green" | "red" | "black" | "brown";

export interface CardPointsRaw {
  ap: number;
  hp: number;
}

export interface CardStoreRaw {
  storeId: string;
  startWhenLastMainQuestStepDone: string;
  finishWhenLastMainQuestStepDone: string;
}

export interface CardRaw {
  imageName: string;
  boosters: number[];
  stores?: CardStoreRaw[];
  type: CardType;
  points?: CardPointsRaw;
}
