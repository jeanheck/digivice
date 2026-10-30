import type { Enemy } from "@/state/models";
import type { MapNpcViewModel } from "@/presentation/viewmodels/map/map-npc.viewmodel";
import type { SeabedRouteLocationViewModel } from "@/presentation/viewmodels/seabed/seabed-route-location.viewmodel";

export type MapMock = {
  mapId: string;
  walking: string[];
  fishing: string[];
  kickingTree: string[];
  npcs: MapNpcViewModel[];
};

export const MapMock: MapMock = {
  mapId: "024B",
  walking: ["105", "106"],
  fishing: ["98", "518"],
  kickingTree: [],
  npcs: [],
};

export type SeabedMapMock = {
  mapId: string;
  seabedRoute: number;
  mapVariant: number;
  directions?: SeabedRouteLocationViewModel;
};

export const SeabedMapMock: SeabedMapMock = {
  mapId: "02E1",
  seabedRoute: 2,
  mapVariant: 1,
};

export const SeabedLongestNamesMapMock: SeabedMapMock = {
  mapId: "02E5",
  seabedRoute: 1,
  mapVariant: 1,
  directions: {
    emerge: [
      { on: "topLeft", location: "024B" },
      { on: "top", location: "0241" },
    ],
    left: ["024B"],
    right: ["0241"],
  },
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
