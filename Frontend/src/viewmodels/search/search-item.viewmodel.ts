export type SearchItemKind = "enemy" | "drop" | "card" | "location" | "store" | "tamer" | "leader" | "npc";

export interface SearchItemViewModel {
  id: string;
  name: string;
  kind?: SearchItemKind;
  kindLabelKey?: string;
  kindLabelParams?: Record<string, string>;
}
