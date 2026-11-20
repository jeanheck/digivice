import type { CardType } from "@/presentation/repositories/tables/raws/tcg/card.raw";

export interface CardViewModel {
  imageName: string;
  nameKey: string;
  noteKey: string;
  type: CardType;
  points?: {
    ap: number;
    hp: number;
  };
}
