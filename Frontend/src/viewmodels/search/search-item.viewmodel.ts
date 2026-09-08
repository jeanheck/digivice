export type SearchItemKind =
  | "enemy"
  | "equipment"
  | "consumableItem"
  | "booster"
  | "card"
  | "location"
  | "store"
  | "tamer"
  | "leader"
  | "npc";

export interface SearchItemViewModel {
  id: string;
  name: string;
  kind?: SearchItemKind;
  kindLabelKey?: string;
  kindLabelParams?: Record<string, string>;
}
