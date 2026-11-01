import type {
  DigievolutionSlot,
  Digimon,
  InBattle,
  Npc,
  Quest,
  Requisite,
  State,
  StoredDigievolution,
} from "@/state/models";
import type { QuestAddresses } from "@/memory/addresses/journals/quest.addresses";
import type { RequisiteAddresses } from "@/memory/addresses/journals/quests/requisite.addresses";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
interface QuestProgress {
  doneSteps?: number[];
  doneRequisites?: string[];
}

function npc(...battles: [id: string, won: boolean][]): Npc {
  return { battles: battles.map(([id, won]) => ({ id, won })) };
}

function stepsUntil(lastStep: number): number[] {
  return Array.from({ length: lastStep }, (_, index) => index + 1);
}

function quest(addresses: QuestAddresses, progress: QuestProgress = {}): Quest {
  const doneSteps = progress.doneSteps ?? [];
  const doneRequisites = progress.doneRequisites ?? [];
  const toRequisite = (requisite: RequisiteAddresses): Requisite => ({
    id: requisite.id,
    isDone: doneRequisites.includes(requisite.id),
  });

  return {
    id: addresses.id,
    requisites: addresses.requisites.map(toRequisite),
    steps: addresses.steps.map((step) => ({
      number: step.number,
      isDone: doneSteps.includes(step.number),
      requisites: step.requisites.map(toRequisite),
    })),
  };
}

function questIn(questsAddresses: QuestAddresses[], id: string, progress?: QuestProgress): Quest {
  const addresses = questsAddresses.find((questAddresses) => questAddresses.id === id);
  if (addresses === undefined) {
    throw new Error(`Quest ${id} not found in the definitions.`);
  }

  return quest(addresses, progress);
}

export function digievolutionSlot(
  index: number,
  digievolutionId: number,
  level: number,
  dvexp: number,
): DigievolutionSlot {
  return { index, digievolutionId, digievolution: { level, dvexp } };
}

export function stored(
  ...entries: [digievolutionId: number, level: number][]
): StoredDigievolution[] {
  return entries.map(([digievolutionId, level]) => ({ digievolutionId, level }));
}

export const OutOfBattle: InBattle = {
  condition: 0,
  strength: 0,
  defense: 0,
  speed: 0,
  hp: { current: 0, max: 0 },
  mp: { current: 0, max: 0 },
};

export const CentralParkKotemon: Digimon = {
  level: 24,
  tp: 5,
  blast: 0,
  experience: 12357,
  hp: { current: 2003, max: 2003 },
  mp: { current: 1230, max: 1230 },
  inBattle: OutOfBattle,
  attributes: { strength: 363, defense: 381, spirit: 196, wisdom: 249, speed: 500, charisma: 167 },
  elements: { fire: 104, water: 154, ice: 97, wind: 100, thunder: 148, machine: 130, dark: 128 },
  equipments: { head: 219, body: 253, right: 178, left: 178, accessory1: 314, accessory2: 314 },
  // Dinohumon, Greymon and Kyukimon (active).
  digievolutions: [
    digievolutionSlot(1, 386, 99, 985),
    digievolutionSlot(2, 5, 1, 0),
    digievolutionSlot(3, 389, 9, 86),
  ],
  storedDigievolutions: stored([386, 99], [5, 1], [389, 9]),
  activeDigievolutionId: 389,
};

const CentralParkPatamon: Digimon = {
  level: 20,
  tp: 10,
  blast: 0,
  experience: 8562,
  hp: { current: 1206, max: 1206 },
  mp: { current: 1508, max: 1508 },
  inBattle: OutOfBattle,
  attributes: { strength: 209, defense: 245, spirit: 417, wisdom: 264, speed: 277, charisma: 143 },
  elements: { fire: 94, water: 146, ice: 121, wind: 142, thunder: 73, machine: 95, dark: 150 },
  equipments: { head: 219, body: 253, right: 134, left: 134, accessory1: null, accessory2: null },
  // Angemon, Digitamamon (active) and Angewomon.
  digievolutions: [
    digievolutionSlot(1, 20, 47, 464),
    digievolutionSlot(2, 56, 58, 574),
    digievolutionSlot(3, 234, 1, 0),
  ],
  storedDigievolutions: stored([20, 47], [56, 58], [234, 1], [211, 1]),
  activeDigievolutionId: 56,
};

const CentralParkRenamon: Digimon = {
  level: 20,
  tp: 5,
  blast: 0,
  experience: 6466,
  hp: { current: 1390, max: 1390 },
  mp: { current: 1443, max: 1443 },
  inBattle: OutOfBattle,
  attributes: { strength: 195, defense: 306, spirit: 339, wisdom: 228, speed: 368, charisma: 174 },
  elements: { fire: 119, water: 127, ice: 143, wind: 92, thunder: 155, machine: 72, dark: 122 },
  equipments: { head: 219, body: 253, right: 185, left: 185, accessory1: null, accessory2: null },
  // Stingmon, Kabuterimon (active) and Taomon.
  digievolutions: [
    digievolutionSlot(1, 260, 61, 607),
    digievolutionSlot(2, 19, 1, 0),
    digievolutionSlot(3, 375, 1, 0),
  ],
  storedDigievolutions: stored([374, 20], [260, 61], [367, 1], [19, 1], [375, 1]),
  activeDigievolutionId: 19,
};

const SideQuests = AddressesRepository.getAllSideQuests();
const LegendaryWeapons = AddressesRepository.getAllLegendaryWeapons();
const DriAgents = AddressesRepository.getAllDriAgents();
const DuelIsland = AddressesRepository.getAllDuelIsland();

export const FieldCentralParkExpected: State = {
  player: {
    bits: 42206,
    mapId: "021D",
    previousMapId: "1000",
    seabedRoute: null,
    mapVariant: null,
  },
  importantItems: {
    treeBoots: true,
    fishingPole: true,
    asukaTrophy: false,
    sunTrophy: false,
  },
  party: {
    slots: [
      { index: 1, digimonId: 0, digimon: CentralParkKotemon },
      { index: 2, digimonId: 7, digimon: CentralParkPatamon },
      { index: 3, digimonId: 6, digimon: CentralParkRenamon },
    ],
  },
  auctions: {
    divineBarrier: true,
    hazardShield: true,
    sniperShield: false,
    dramonShield: true,
    yinYangWand: false,
    hazardFang: false,
  },
  cardBattle: { id: null },
  digimonBattle: { field: 0, enemy: null },
  npcs: {
    genji: npc(["first", true], ["second", false]),
    natsumi: npc(["first", false]),
    catherine: npc(["first", false]),
    lucia: npc(["first", false]),
    robert: npc(["first", false]),
    akiba: npc(["first", false]),
    chris: npc(["first", false]),
    tomomi: npc(["first", false]),
    mitch: npc(["first", false]),
    bob: npc(["first", false]),
    andy: npc(["first", false]),
    george: npc(["first", false]),
    meiLin: npc(["first", false]),
    jessica: npc(["first", false]),
    gordon: npc(["first", false]),
    alice: npc(["first", false]),
    nakano: npc(["first", false]),
    seiryuLeader: npc(["first", true]),
    keith: npc(["first", true]),
    suzakuLeader: npc(["first", true]),
    fakeByakkoLeader: npc(["first", true]),
    byakkoLeader: npc(["first", true]),
    aoaAttacker: npc(["first", true]),
    optionalTrooper1AdminCenterB1F: npc(["first", false]),
    optionalTrooper2AdminCenterB1F: npc(["first", false]),
    optionalTrooperAdminFloor1: npc(["first", false]),
    optionalTrooper1AdminFloor2: npc(["first", false]),
    optionalSoldier1AdminFloor2: npc(["first", false]),
    optionalTrooper2AdminFloor2: npc(["first", false]),
  },
  journal: {
    // RAM has steps 6 and 21 unset; the main quest cascade marks them as done.
    mainQuest: quest(AddressesRepository.getMainQuest(), {
      doneSteps: stepsUntil(61),
      doneRequisites: ["tntBall"],
    }),
    sideQuests: [
      questIn(SideQuests, "fishingPole", {
        doneSteps: [1, 2, 3],
        doneRequisites: ["folderBag", "spiderWeb", "redSnapper"],
      }),
      questIn(SideQuests, "folderBag", { doneSteps: [1] }),
      questIn(SideQuests, "treeBoots", {
        doneSteps: stepsUntil(7),
        doneRequisites: ["folderBag"],
      }),
    ],
    legendaryWeapons: [
      questIn(LegendaryWeapons, "eternally", { doneSteps: [1] }),
      questIn(LegendaryWeapons, "invincible", { doneSteps: [1], doneRequisites: ["seiryuLeader"] }),
      questIn(LegendaryWeapons, "muramasa", { doneRequisites: ["smellHerb"] }),
      questIn(LegendaryWeapons, "punishment", { doneSteps: [1], doneRequisites: ["numemon"] }),
      questIn(LegendaryWeapons, "superNova", { doneRequisites: ["submarimon"] }),
    ],
    driAgents: [
      questIn(DriAgents, "driAgentAgumon", {
        doneSteps: [1, 2, 3],
        doneRequisites: ["seiryuLeader", "agumonDDNA"],
      }),
      questIn(DriAgents, "driAgentGuilmon", {
        doneSteps: [1, 2, 3],
        doneRequisites: ["seiryuLeader", "guilmonDDNA"],
      }),
      questIn(DriAgents, "driAgentKotemon", { doneSteps: [3], doneRequisites: ["submarimon"] }),
      questIn(DriAgents, "driAgentKumamon", {
        doneSteps: [1, 2, 3],
        doneRequisites: ["submarimon", "kumamonDDNA"],
      }),
      questIn(DriAgents, "driAgentMonmon", {
        doneSteps: [1, 2, 3],
        doneRequisites: ["submarimon", "monmonDDNA"],
      }),
      questIn(DriAgents, "driAgentPatamon", { doneSteps: [3], doneRequisites: ["submarimon"] }),
      questIn(DriAgents, "driAgentRenamon", { doneSteps: [3], doneRequisites: ["submarimon"] }),
      questIn(DriAgents, "driAgentVeemon", { doneRequisites: ["byakkoLeader"] }),
    ],
    duelIsland: [
      questIn(DuelIsland, "asukaRematch"),
      questIn(DuelIsland, "asukaTrophy", { doneRequisites: ["submarimon"] }),
    ],
  },
};
