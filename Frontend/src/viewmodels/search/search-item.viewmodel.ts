export type SearchItemKind = "enemy" | "drop" | "card";

export interface SearchItemViewModel {
  id: string;
  name: string;
  kind?: SearchItemKind;
}
