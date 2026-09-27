export interface FolderCardRaw {
  id: string;
  quantity: number;
}

export interface FolderRaw {
  level: number;
  cards: FolderCardRaw[];
}
