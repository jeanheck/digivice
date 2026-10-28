import type { Enemy } from "@/state/models";

export type MapMock = {
  mapId: string;
  walking: string[];
  fishing: string[];
  kickingTree: string[];
};

export const MapMock: MapMock = {
  mapId: "023A",
  walking: ["75", "90", "91", "92", "184"],
  fishing: ["103", "516"],
  kickingTree: ["85", "184", "517"],
};

export type SeabedMapMock = {
  mapId: string;
  seabedRoute: number;
  mapVariant: number;
};

export const SeabedMapMock: SeabedMapMock = {
  mapId: "02E5",
  seabedRoute: 1,
  mapVariant: 1,
};

export type MobiusDesertMapMock = {
  mapId: string;
  mapVariant: number;
  walking: string[];
};

export const MobiusDesertMapMock: MobiusDesertMapMock = {
  mapId: "0258",
  mapVariant: 1,
  walking: ["110", "111", "193"],
};

export type CardBattleMapMock = {
  cardBattleId: number;
};

export const CardBattleMapMock: CardBattleMapMock = {
  cardBattleId: 1,
};

export type DigimonBattleMapMock = {
  fieldId: number;
  enemy: Enemy;
};

export const DigimonBattleMapMock: DigimonBattleMapMock = {
  fieldId: 8,
  enemy: {
    id: 37,
    groupId: 55,
    condition: 0,
    strength: 200,
    defense: 175,
    speed: 150,
    hp: { current: 210, max: 336 },
  },
};
