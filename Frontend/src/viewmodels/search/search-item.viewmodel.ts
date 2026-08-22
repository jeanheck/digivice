export type SearchItemKind = "enemy" | "drop" | "card" | "location";

export interface SearchItemViewModel {
  id: string;
  name: string;
  kind?: SearchItemKind;
}
