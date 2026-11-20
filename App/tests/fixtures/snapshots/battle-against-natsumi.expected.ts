import type { State } from "@/state/models";
import {
  CentralParkKotemon,
  digievolutionSlot,
  FieldCentralParkExpected,
  stored,
} from "./field-central-park.expected";

export const BattleAgainstNatsumiExpected: Pick<State, "party" | "digimonBattle" | "journal"> = {
  party: {
    slots: [
      {
        index: 1,
        digimonId: 0,
        digimon: {
          ...CentralParkKotemon,
          experience: 12363,
          inBattle: {
            condition: 0,
            strength: 0,
            defense: 0,
            speed: 0,
            hp: { current: 1989, max: 2003 },
            mp: { current: 1230, max: 1230 },
          },
          equipments: {
            head: 219,
            body: 263,
            right: 178,
            left: 178,
            accessory1: null,
            accessory2: null,
          },
          digievolutions: [
            digievolutionSlot(1, 386, 99, 985),
            digievolutionSlot(2, 5, 1, 0),
            digievolutionSlot(3, 389, 9, 87),
          ],
          storedDigievolutions: stored([386, 99], [5, 1], [389, 9]),
        },
      },
      { index: 2, digimonId: null, digimon: null },
      { index: 3, digimonId: null, digimon: null },
    ],
  },
  digimonBattle: {
    field: 0,
    enemy: {
      id: 25,
      groupId: 201,
      condition: 0,
      strength: 0,
      defense: 0,
      speed: 0,
      hp: { current: 192, max: 192 },
    },
  },
  journal: FieldCentralParkExpected.journal,
};
