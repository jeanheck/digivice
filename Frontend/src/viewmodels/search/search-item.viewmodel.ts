export type SearchItemKind = "enemy" | "drop";

export interface SearchItemViewModel {
  id: string;
  name: string;
  kind?: SearchItemKind;
}
