export type DesertNeighborNameViewModel =
  | { kind: "literal"; value: string }
  | { kind: "i18n"; key: string };
