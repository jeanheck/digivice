import type { Vital } from "@/state/models";
import type { DigimonAttribute } from "@/shared/constants/digimon-attribute.constant";
import type { DigimonElement } from "@/shared/constants/digimon-element.constant";
import type { EquipmentSlot } from "@/shared/types/equipment-slot.type";

export type PartyDigievolutionMock = {
  digievolutionId: number;
  level: number;
  dvexpInLevel: number;
  dvexpPerLevel: number;
  isDnaActive: boolean;
};

export type PartyDigievolutionSlotMock = PartyDigievolutionMock | null;

export type PartyStatMock = {
  value: number;
  digievolutionBonus: number;
  battleDelta: number;
};

export type PartyStatsMock = {
  attributes: Record<DigimonAttribute, PartyStatMock>;
  elements: Record<DigimonElement, PartyStatMock>;
};

export type PartyEquipmentsMock = Record<EquipmentSlot, number | null>;

export type PartyDigimonMock = {
  digimonId: number;
  name: string;
  level: number;
  experience: number;
  hp: Vital;
  mp: Vital;
  tp: number;
  blast: number;
  digievolutions: PartyDigievolutionSlotMock[];
  activeDigievolutionId: number | null;
  stats: PartyStatsMock;
  equipments: PartyEquipmentsMock;
};

export type PartySlotMock = PartyDigimonMock | null;

const DigievolutionsMock: PartyDigievolutionSlotMock[] = [
  { digievolutionId: 260, level: 61, dvexpInLevel: 2, dvexpPerLevel: 10, isDnaActive: true },
  { digievolutionId: 381, level: 1, dvexpInLevel: 5, dvexpPerLevel: 10, isDnaActive: true },
  null,
];

const createStat = (value: number, digievolutionBonus = 0, battleDelta = 0): PartyStatMock => {
  return { value, digievolutionBonus, battleDelta };
};

export const PartySlotsMock: PartySlotMock[] = [
  {
    digimonId: 5,
    name: "Guilmon",
    level: 20,
    experience: 6900,
    hp: { current: 180, max: 240 },
    mp: { current: 30, max: 120 },
    tp: 12,
    blast: 0,
    digievolutions: DigievolutionsMock,
    activeDigievolutionId: 381,
    stats: {
      attributes: {
        strength: createStat(142, 30, 20),
        defense: createStat(98, 25),
        spirit: createStat(76, 10),
        wisdom: createStat(64),
        speed: createStat(110, 15, -12),
        charisma: createStat(35),
      },
      elements: {
        fire: createStat(48, 12),
        water: createStat(22),
        ice: createStat(18),
        wind: createStat(30, 5),
        thunder: createStat(26),
        machine: createStat(40, 8),
        dark: createStat(15),
      },
    },
    equipments: {
      head: 12,
      body: 34,
      right: 56,
      left: null,
      accessory1: 78,
      accessory2: null,
    },
  },
  {
    digimonId: 6,
    name: "Renamon",
    level: 18,
    experience: 4550,
    hp: { current: 50, max: 200 },
    mp: { current: 140, max: 160 },
    tp: 5,
    blast: 0,
    digievolutions: DigievolutionsMock,
    activeDigievolutionId: 381,
    stats: {
      attributes: {
        strength: createStat(84, 18),
        defense: createStat(72, 0, -15),
        spirit: createStat(128, 30),
        wisdom: createStat(115, 20),
        speed: createStat(134, 10, 18),
        charisma: createStat(52),
      },
      elements: {
        fire: createStat(20),
        water: createStat(34, 6),
        ice: createStat(44, 10),
        wind: createStat(50, 12),
        thunder: createStat(18),
        machine: createStat(12),
        dark: createStat(38, 8),
      },
    },
    equipments: {
      head: null,
      body: null,
      right: 21,
      left: 43,
      accessory1: 65,
      accessory2: 87,
    },
  },
  {
    digimonId: 1,
    name: "Kumamon",
    level: 15,
    experience: 3800,
    hp: { current: 260, max: 260 },
    mp: { current: 90, max: 110 },
    tp: 0,
    blast: 0,
    digievolutions: DigievolutionsMock,
    activeDigievolutionId: 381,
    stats: {
      attributes: {
        strength: createStat(96),
        defense: createStat(120, 22),
        spirit: createStat(60),
        wisdom: createStat(48),
        speed: createStat(58),
        charisma: createStat(28),
      },
      elements: {
        fire: createStat(10),
        water: createStat(36),
        ice: createStat(62, 15),
        wind: createStat(14),
        thunder: createStat(20),
        machine: createStat(24),
        dark: createStat(8),
      },
    },
    equipments: {
      head: 15,
      body: null,
      right: null,
      left: null,
      accessory1: null,
      accessory2: null,
    },
  },
];
