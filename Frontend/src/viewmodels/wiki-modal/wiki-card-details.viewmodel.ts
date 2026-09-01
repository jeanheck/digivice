import type { CardType } from "@/repositories/tables/raws/tcg/card.raw";

export interface WikiCardDetailsViewModel {
  imageName: string;
  nameKey: string;
  noteKey: string;
  type: CardType;
  points?: {
    ap: number;
    hp: number;
  };
}
