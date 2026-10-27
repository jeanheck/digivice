import type { CardType } from "@/repositories/tables/raws/tcg/card.raw";

export interface CardAlbumItemViewModel {
  cardId: string;
  cardIndex: number; // 1 to 314
  imageName: string;
  nameKey: string;
  noteKey: string;
  type: CardType;
  points?: {
    ap: number;
    hp: number;
  };
  quantityOwned: number;
  isOwned: boolean;
}

export interface CardAlbumPageViewModel {
  page: number; // 1 to 27
  totalPages: number; // 27
  cards: (CardAlbumItemViewModel | null)[]; // exactly 12 items (null for empty slots on page 27)
  totalUniqueOwned: number;
  totalCards: number; // 314
  totalCopies: number;
}

export type BoosterSeries = "A" | "B" | "R";

export interface CardAlbumBoosterItemViewModel {
  id: number;
  code: string;
  nameKey: string;
  series: BoosterSeries;
  totalCards: number;
  ownedCards: number;
  percentage: string;
  isComplete: boolean;
}
