export interface MobiusDesertAreaMapCellRaw {
  label: string;
  north: string;
  east: string;
  south: string;
  west: string;
  coordinates?: {
    x: number;
    y: number;
  };
}
