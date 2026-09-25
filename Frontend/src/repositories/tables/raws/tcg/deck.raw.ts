export interface DeckCardRaw {
  id: string;
  quantity: number;
}

export interface DeckRaw {
  level: number;
  cards: DeckCardRaw[];
}
